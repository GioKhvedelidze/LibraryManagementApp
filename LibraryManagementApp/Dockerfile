FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LibraryManagementApp/LibraryManagementApp.csproj", "LibraryManagementApp/"]
RUN dotnet restore "LibraryManagementApp/LibraryManagementApp.csproj"
COPY . .
WORKDIR "/src/LibraryManagementApp"
RUN dotnet build "LibraryManagementApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LibraryManagementApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/LibraryManagementApp.xml .
ENTRYPOINT ["dotnet", "LibraryManagementApp.dll"]