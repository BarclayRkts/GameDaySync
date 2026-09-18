FROM ://microsoft.com AS base
WORKDIR /app
EXPOSE 8080

FROM ://microsoft.com AS build
WORKDIR /src

COPY ["GameDay-Sync/GameDay-Sync.csproj", "GameDay-Sync/"]
RUN dotnet restore "GameDay-Sync/GameDay-Sync.csproj"

COPY . .
WORKDIR "/src/GameDay-Sync"
RUN dotnet build "GameDay-Sync.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameDay-Sync.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameDay-Sync.dll"]