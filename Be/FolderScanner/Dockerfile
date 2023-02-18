#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["FolderScanner/FolderScanner.csproj", "FolderScanner/"]
RUN dotnet restore "FolderScanner/FolderScanner.csproj"
COPY . .
WORKDIR "/src/FolderScanner"
RUN dotnet build "FolderScanner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FolderScanner.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FolderScanner.dll"]