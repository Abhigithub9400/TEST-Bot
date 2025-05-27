FROM ubuntu:22.04

# Install necessary packages and .NET SDK using Microsoft's official repository
RUN apt-get update && \
    apt-get -y purge nginx && \
    apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    gnupg \
    lsb-release \
    software-properties-common \
    apache2 \
    wget && \
    # Add Microsoft package repository
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    # Install .NET SDK 8.0
    apt-get install -y dotnet-sdk-8.0 && \
    # Install Node.js
    curl -sL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    npm install -g npm@latest && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Verify .NET SDK installation
RUN dotnet --version

# Enable Apache modules
RUN a2enmod proxy proxy_http proxy_balancer lbmethod_byrequests rewrite

# Set working directory
WORKDIR /app

# Copy project files
COPY ["MediAssist.UI/MediAssist.UI.csproj", "MediAssist.UI/"]
COPY ["MediAssist.Application/MediAssist.Application.csproj", "MediAssist.Application/"]
COPY ["MediAssist.Application.Abstract/MediAssist.Application.Abstract.csproj", "MediAssist.Application.Abstract/"]
COPY ["MediAssist.Infrastructure.Abstract/MediAssist.Infrastructure.Abstract.csproj", "MediAssist.Infrastructure.Abstract/"]
COPY ["MediAssist.Dependency/MediAssist.Dependency.csproj", "MediAssist.Dependency/"]
COPY ["MediAssist.Configurations/MediAssist.Configurations.csproj", "MediAssist.Configurations/"]

# Restore NuGet packages
RUN dotnet restore "./MediAssist.UI/MediAssist.UI.csproj"

# Copy the rest of the application
COPY . .

# Set working directory for npm install
WORKDIR "MediAssist.UI/ClientApp"

# Increase Node.js memory limit
ENV NODE_OPTIONS=--max_old_space_size=4096

# Clear npm cache and install npm packages
RUN npm cache clean --force && \
    npm install --verbose --no-audit --legacy-peer-deps || \
    (cat /root/.npm/_logs/*-debug.log && exit 1)

RUN npm run build

# Set working directory back to the main project
WORKDIR "/app/MediAssist.UI"

# Build and publish the application
RUN dotnet build "./MediAssist.UI.csproj" -c Release -o /app/build && \
    dotnet publish "./MediAssist.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Copy configuration files
COPY CI-CD/apache.conf /etc/apache2/sites-available/000-default.conf
COPY CI-CD/entrypoint.sh /app/entrypoint.sh

# Ensure the entrypoint script uses LF line endings and is executable
RUN sed -i 's/\r$//' /app/entrypoint.sh && \
    chmod +x /app/entrypoint.sh

# Expose port 80
EXPOSE 80

# Set the entrypoint
CMD ["/app/entrypoint.sh"]