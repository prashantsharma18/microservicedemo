#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/services/service2/Service2.API/Service2.API.csproj", "src/services/service2/Service2.API/"]
RUN dotnet restore "src/services/service2/Service2.API/Service2.API.csproj"
COPY . .
WORKDIR "/src/src/services/service2/Service2.API"
RUN dotnet build "Service2.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service2.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service2.API.dll"]