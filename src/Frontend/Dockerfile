# escape=`

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR C:\src

COPY ["src/Frontend/Frontend.csproj", "src/Frontend/"]
RUN dotnet restore "src/Frontend/Frontend.csproj"

COPY . .
WORKDIR C:\src\src\Frontend
RUN dotnet publish "Frontend.csproj" --configuration Release --output C:\app\publish --no-restore

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS final
WORKDIR C:\app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build C:\app\publish .
ENTRYPOINT ["dotnet", "Frontend.dll"]
