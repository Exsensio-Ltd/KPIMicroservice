FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV CONTEXT_URL="http://172.17.0.1:1026"
ENV ASPNETCORE_URLS="https://+:443;http://+:80"
ENV ASPNETCORE_HTTPS_PORT="443"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OEEMicroservice.csproj", "./"]
RUN dotnet restore "OEEMicroservice.csproj"
COPY . .
COPY debts /src
WORKDIR "/src/"
RUN dotnet build "OEEMicroservice.csproj" -c Release -o /app
RUN dotnet dev-certs https --trust

FROM build AS publish
RUN dotnet publish "OEEMicroservice.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OEEMicroservice.dll"]