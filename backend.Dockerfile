FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app


COPY . ./
RUN dotnet restore


RUN dotnet publish src/Web/Web.csproj -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./


ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80



ENTRYPOINT ["dotnet", "Web.dll"]



