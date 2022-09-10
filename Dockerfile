FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["com.tweetapp.Controller/com.tweetapp.Controller.csproj", "com.tweetapp.Controller/"]
COPY ["com.tweetapp.Model/com.tweetapp.Model.csproj", "com.tweetapp.Model/"]
COPY ["com.tweetapp.Repository/com.tweetapp.Repository.csproj", "com.tweetapp.Repository/"]
COPY ["com.tweetapp.Services/com.tweetapp.Services.csproj", "com.tweetapp.Services/"]
RUN dotnet restore "com.tweetapp.Controller/com.tweetapp.Controller.csproj"
COPY . .
WORKDIR "/src/com.tweetapp.Controller"
RUN dotnet build "com.tweetapp.Controller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "com.tweetapp.Controller.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "com.tweetapp.Controller.dll"]
