using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediAssist.DbContext
{
    public class MediAssistDbContext : IdentityDbContext<ApplicationUser>
    {
        public MediAssistDbContext(DbContextOptions<MediAssistDbContext> options) :
            base(options)
        { }

        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<UserTitle> UserTitles { get; set; }
        public DbSet<UserGender> Genders { get; set; }
        public DbSet<DoctorMedicalCredentials> MedicalCredentials { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FeedbackCategoryMapping> feedbackCategoryMappings { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<UsersHistory> UsersHistories { get; set; }
        public DbSet<DemoRequestLog> DemoRequestLogs { get; set; }
        public DbSet<Master_Features> Master_Features { get; set; }
        public DbSet<Master_Plans> Master_Plans { get; set; }
        public DbSet<FeaturePlanConfiguration> FeaturePlanConfiguration { get; set; }
        public DbSet<UserConfiguration> UserConfiguration { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<Master_EmailTemplate> Master_EmailTemplates { get; set; }
        public DbSet<MediAssistLogs> mediAssistLogs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<FHIRStoreMapping> FHIRStoreMapping { get; set; }
        public DbSet<DoctorPatientMapping> DoctorPatientMapping { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(x =>
            {
                x.ToTable("Users");
            });

            builder.Entity<DoctorProfile>()
                .HasOne(dp => dp.UserTitle)
                .WithMany()
                .HasForeignKey(dp => dp.Title)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DoctorProfile>()
                .HasOne(dp => dp.UserGender)
                .WithMany()
                .HasForeignKey(dp => dp.Gender)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DoctorProfile>()
                .HasOne(dp => dp.MedicalCredential)
                .WithMany()
                .HasForeignKey(dp => dp.MedicalCredentials)
                .OnDelete(DeleteBehavior.Restrict);

           
            builder.Entity<Feedback>()
            .Property(f => f.UserId)
            .IsRequired(false);

            builder.Entity<Feedback>()
            .Property(f => f.EmailAddress)
            .IsRequired();

            builder.Entity<FeedbackCategoryMapping>()
                .HasOne(dp => dp.Category)
                .WithMany()
                .HasForeignKey(dp => dp.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FeedbackCategoryMapping>()
                .HasOne(dp => dp.Feedback)
                .WithMany()
                .HasForeignKey(dp => dp.FeedbackID)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}
