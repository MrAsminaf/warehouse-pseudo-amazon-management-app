FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["Warehouse.WebAPI/Warehouse.WebAPI.csproj", "Warehouse.WebAPI/"]
COPY ["Warehouse.Domain/Warehouse.Domain.csproj", "Warehouse.Domain/"]
COPY ["Warehouse.Infrastructure/Warehouse.Infrastructure.csproj", "Warehouse.Infrastructure/"]
COPY ["Warehouse.Application/Warehouse.Application.csproj", "Warehouse.Application/"]
RUN dotnet restore "Warehouse.WebAPI/Warehouse.WebAPI.csproj"

COPY . .
WORKDIR "/src/Warehouse.WebAPI"
RUN dotnet build "Warehouse.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Warehouse.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Warehouse.WebAPI.dll"]
