FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY ["Server/TaskTracker.Server.csproj", "Server/"]
COPY ["Client/TaskTracker.Client.csproj", "Client/"]
COPY ["Shared/TaskTracker.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/TaskTracker.Server.csproj"

COPY . .
WORKDIR "/app/Server"
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "TaskTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

WORKDIR /app/publish
ENTRYPOINT ["dotnet", "TaskTracker.Server.dll"]