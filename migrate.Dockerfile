FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


WORKDIR /app

RUN dotnet tool install --global dotnet-ef


ENV PATH="$PATH:/root/.dotnet/tools"


COPY . .


CMD ["dotnet", "ef", "database", "update", "--project", "src/Infrastructure/Infrastructure.csproj", "--startup-project", "src/Web/Web.csproj"]
