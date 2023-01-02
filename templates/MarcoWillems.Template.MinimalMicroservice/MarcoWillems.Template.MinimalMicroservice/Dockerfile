#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MarcoWillems.Template.MinimalMicroservice/MarcoWillems.Template.MinimalMicroservice.csproj", "MarcoWillems.Template.MinimalMicroservice/"]
COPY ["MarcoWillems.Template.MinimalMicroservice.Contracts/MarcoWillems.Template.MinimalMicroservice.Contracts.csproj", "MarcoWillems.Template.MinimalMicroservice.Contracts/"]
COPY ["MarcoWillems.Template.MinimalMicroservice.Services/MarcoWillems.Template.MinimalMicroservice.Services.csproj", "MarcoWillems.Template.MinimalMicroservice.Services/"]
COPY ["MarcoWillems.Template.MinimalMicroservice.Database/MarcoWillems.Template.MinimalMicroservice.Database.csproj", "MarcoWillems.Template.MinimalMicroservice.Database/"]
RUN dotnet restore "MarcoWillems.Template.MinimalMicroservice/MarcoWillems.Template.MinimalMicroservice.csproj"
COPY . .
WORKDIR "/src/MarcoWillems.Template.MinimalMicroservice"
RUN dotnet build "MarcoWillems.Template.MinimalMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarcoWillems.Template.MinimalMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarcoWillems.Template.MinimalMicroservice.dll"]
