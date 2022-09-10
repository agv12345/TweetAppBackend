FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TweetApp.Controller/TweetApp.Controller.csproj", "TweetApp.Controller/"]
RUN dotnet restore "TweetApp.Controller/TweetApp.Controller.csproj"
COPY . .
WORKDIR "/src/TweetApp.Controller"
RUN dotnet build "TweetApp.Controller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TweetApp.Controller.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TweetApp.Controller.dll"]
