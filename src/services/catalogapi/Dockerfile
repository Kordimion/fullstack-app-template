FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY catalogapi.csproj ./
RUN dotnet restore catalogapi.csproj
COPY . .
RUN dotnet build catalogapi.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish catalogapi.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "catalogapi.dll"]