using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using MediAssist.Configurations;
using MediAssist.DbContext;
using MediAssist.Dependency;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.HttpProvider;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration files and env vars
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Read the full vault URL from config 
var vaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
if (string.IsNullOrWhiteSpace(vaultUrl))
    throw new InvalidOperationException("Missing AzureKeyVault:VaultUrl in configuration.");

TokenCredential credential = builder.Environment.IsProduction()
    ? new DefaultAzureCredential()
    : new ClientSecretCredential(
        builder.Configuration["AzureAd:TenantId"]!,
        builder.Configuration["AzureAd:ClientId"]!,
        builder.Configuration["AzureAd:ClientSecret"]!
      );

builder.Configuration.AddAzureKeyVault(new Uri(vaultUrl), credential);
var secretClient = new SecretClient(new Uri(vaultUrl), credential);

builder.Services.AddSingleton<IAppSettings, Appsettings>();

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services
builder.Services.AddDependencies();
builder.Services.AddInfrastuctureHttpProvider();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IAzureKeyVaultService, AzureKeyVaultService>();
builder.Services.AddSingleton(secretClient);

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuth["ClientId"];
    options.ClientSecret = googleAuth["ClientSecret"];
    options.CallbackPath = googleAuth["CallbackPath"];
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtToken:Issuer"],
        ValidAudience = builder.Configuration["JwtToken:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:Key"]))
    };
});

// DB Context & Identity
var connectionString = builder.Configuration.GetConnectionString("SqlConnectionString");
builder.Services.AddDbContext<MediAssistDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<MediAssistDbContext>()
        .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(1); // Set token lifespan to 1 hour
});

// Hangfire
builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

// Configure authorization
builder.Services.AddAuthorization();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter{ 
            User =  builder.Configuration.GetSection("HangfireSettings:UserName").Value,
            Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
        }
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// Fallback for SPA
app.MapFallbackToFile("index.html", new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
        ctx.Context.Response.Headers.Append("Expires", "-1");
    }
});

app.Run();
