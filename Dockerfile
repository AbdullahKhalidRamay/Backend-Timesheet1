FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TimeSheetAPI.csproj", "./"]
RUN dotnet restore "TimeSheetAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "TimeSheetAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeSheetAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeSheetAPI.dll"]