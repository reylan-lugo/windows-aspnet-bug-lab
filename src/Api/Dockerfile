# escape=`

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR C:\src

COPY ["src/Api/Api.csproj", "src/Api/"]
RUN dotnet restore "src/Api/Api.csproj"

COPY . .
WORKDIR C:\src\src\Api
RUN dotnet publish "Api.csproj" --configuration Release --output C:\app\publish --no-restore

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS final
WORKDIR C:\app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build C:\app\publish .
ENTRYPOINT ["dotnet", "Api.dll"]
