#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Loader.API/Loader.API.csproj", "Services/Loader.API/"]
RUN dotnet restore "Services/Loader.API/Loader.API.csproj"
COPY . .
WORKDIR "/src/Services/Loader.API"
RUN dotnet build "Loader.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Loader.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Loader.API.dll"]