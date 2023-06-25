FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TranslationManagement.Tests/TranslationManagement.Tests.csproj", "TranslationManagement.Tests/"]
RUN dotnet restore "TranslationManagement.Tests/TranslationManagement.Tests.csproj"
COPY . .
WORKDIR "/src/TranslationManagement.Tests"
RUN dotnet build "TranslationManagement.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TranslationManagement.Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TranslationManagement.Tests.dll"]
