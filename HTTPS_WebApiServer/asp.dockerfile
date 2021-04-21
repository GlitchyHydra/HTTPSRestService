FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /source

# Copy the csproj file and restore any dependecies (via NUGET)
COPY FreelancerWeb.sln ./
COPY *.csproj ./
COPY wait-for-it.sh ./
RUN dotnet restore

# copy everything else and build app
COPY . ./freelancerApi/
WORKDIR /source/freelancerApi
RUN dotnet publish -c Release -o /app

#Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 8002 8003
COPY --from=build-env /app ./
ENTRYPOINT ["dotnet", "FreelancerWeb.dll"]

#9b4a9c5c-b2ac-49f6-b35b-a899c2d5e644