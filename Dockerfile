# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file(s)
COPY TAE.UploadService/*.csproj ./TAE.UploadService/

# Restore
RUN dotnet restore ./TAE.UploadService/TAE.UploadService.csproj

# Copy the rest of the source
COPY . .

# Build
RUN dotnet build ./TAE.UploadService/TAE.UploadService.csproj -c Release --no-restore

# Publish
RUN dotnet publish ./TAE.UploadService/TAE.UploadService.csproj -c Release -o /app/publish --no-build

# Stage 2: Runtime container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TAE.UploadService.dll"]
