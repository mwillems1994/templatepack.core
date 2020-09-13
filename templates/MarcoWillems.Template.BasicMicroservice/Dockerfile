FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy necessary files and restore as distinct layer
COPY MarcoWillems.Template.BasicMicroservice.API/MarcoWillems.Template.BasicMicroservice.API.csproj ./MarcoWillems.Template.BasicMicroservice.API/
COPY MarcoWillems.Template.BasicMicroservice.Database/MarcoWillems.Template.BasicMicroservice.Database.csproj ./MarcoWillems.Template.BasicMicroservice.Database/
COPY MarcoWillems.Template.BasicMicroservice.Services/MarcoWillems.Template.BasicMicroservice.Services.csproj ./MarcoWillems.Template.BasicMicroservice.Services/

#WORKDIR ./CustomerManagementAPI.Nl
RUN dotnet restore MarcoWillems.Template.BasicMicroservice.API/

# Copy everything else and build
COPY . ./
#RUN dotnet publish -c Release -o out
RUN ls
RUN dotnet publish MarcoWillems.Template.BasicMicroservice.API/ --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build-env /app/out .

# Expose ports
EXPOSE 5100/tcp
ENV ASPNETCORE_URLS http://*:5100
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "MarcoWillems.Template.BasicMicroservice.API.dll"]
