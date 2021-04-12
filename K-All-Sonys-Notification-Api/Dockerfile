#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["K-All-Sonys-Notification-Api/K-All-Sonys-Notification-Api.csproj", "K-All-Sonys-Notification-Api/"]
RUN dotnet restore "K-All-Sonys-Notification-Api/K-All-Sonys-Notification-Api.csproj"
COPY . .
WORKDIR "/src/K-All-Sonys-Notification-Api"
RUN dotnet build "K-All-Sonys-Notification-Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "K-All-Sonys-Notification-Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K-All-Sonys-Notification-Api.dll"]