# Use the .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files into the container
COPY . .

# Install the dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef

# Ensure the global tools are in the PATH
ENV PATH="$PATH:/root/.dotnet/tools"



# Run the EF database update command directly
CMD ["dotnet", "ef", "database", "update", "--project", "src/Infrastructure/Infrastructure.csproj", "--startup-project", "src/Web/Web.csproj"]
