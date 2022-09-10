FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TweetApp.Repository/TweetApp.Repository.csproj", "TweetApp.Repository/"]
RUN dotnet restore "TweetApp.Repository/TweetApp.Repository.csproj"
COPY . .
WORKDIR "/src/TweetApp.Repository"
RUN dotnet build "TweetApp.Repository.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TweetApp.Repository.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TweetApp.Repository.dll"]
