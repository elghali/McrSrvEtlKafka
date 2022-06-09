#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Parser.API/Parser.API.csproj", "Services/Parser.API/"]
RUN dotnet restore "Services/Parser.API/Parser.API.csproj"
COPY . .
WORKDIR "/src/Services/Parser.API"
RUN dotnet build "Parser.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Parser.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Parser.API.dll"]