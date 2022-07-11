#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MarcoWillems.Template.WebApi/MarcoWillems.Template.WebApi.csproj", "MarcoWillems.Template.WebApi/"]
RUN dotnet restore "MarcoWillems.Template.WebApi/MarcoWillems.Template.WebApi.csproj"
COPY . .
WORKDIR "/src/MarcoWillems.Template.WebApi"
RUN dotnet build "MarcoWillems.Template.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarcoWillems.Template.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarcoWillems.Template.WebApi.dll"]