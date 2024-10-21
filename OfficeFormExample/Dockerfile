FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OfficeFormExample/OfficeFormExample.csproj", "OfficeFormExample/"]
RUN dotnet restore "OfficeFormExample/OfficeFormExample.csproj"
COPY . .
WORKDIR "/src/OfficeFormExample"
RUN dotnet publish "OfficeFormExample.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OfficeFormExample.dll"]
