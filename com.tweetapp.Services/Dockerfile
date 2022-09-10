FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TweetApp.Services/TweetApp.Services.csproj", "TweetApp.Services/"]
RUN dotnet restore "TweetApp.Services/TweetApp.Services.csproj"
COPY . .
WORKDIR "/src/TweetApp.Services"
RUN dotnet build "TweetApp.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TweetApp.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TweetApp.Services.dll"]
