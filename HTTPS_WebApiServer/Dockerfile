FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app 

# Copy the csproj file and restore any dependecies (via NUGET)
COPY *.sln .
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet publish -c Release -o out

#Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
EXPOSE 8002 8003
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "HTTTPS_WebApiServer.dll"]

#9b4a9c5c-b2ac-49f6-b35b-a899c2d5e644