FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Innowise.Clinic.Profiles.API/Innowise.Clinic.Profiles.API.csproj", "Innowise.Clinic.Profiles.API/"]
COPY ["Innowise.Clinic.Profiles.Persistence/Innowise.Clinic.Profiles.Persistence.csproj", "Innowise.Clinic.Profiles.Persistence/"]
COPY ["Innowise.Clinic.Profiles.Dto/Innowise.Clinic.Profiles.Dto.csproj", "Innowise.Clinic.Profiles.Dto/"]
COPY ["Innowise.Clinic.Profiles.Services/Innowise.Clinic.Profiles.Services.csproj", "Innowise.Clinic.Profiles.Services/"]
RUN dotnet restore "Innowise.Clinic.Profiles.API/Innowise.Clinic.Profiles.API.csproj"
COPY . .
WORKDIR "/src/Innowise.Clinic.Profiles.API"
RUN dotnet build "Innowise.Clinic.Profiles.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innowise.Clinic.Profiles.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innowise.Clinic.Profiles.API.dll"]
