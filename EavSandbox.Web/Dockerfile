FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY EavSandbox.Web/EavSandbox.Web.csproj EavSandbox.Web/
RUN dotnet restore EavSandbox.Web/EavSandbox.Web.csproj
COPY . .
WORKDIR /src/EavSandbox.Web
RUN dotnet build EavSandbox.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish EavSandbox.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EavSandbox.Web.dll"]
