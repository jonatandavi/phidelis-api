#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["phidelis-api/phidelis-api.csproj", "phidelis-api/"]
RUN dotnet restore "phidelis-api/phidelis-api.csproj"
COPY . .
WORKDIR "/src/phidelis-api"
RUN dotnet build "phidelis-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "phidelis-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "phidelis-api.dll"]