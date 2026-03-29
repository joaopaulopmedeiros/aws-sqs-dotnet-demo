FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Order.Core/Order.Core.csproj Order.Core/
COPY src/Order.Infrastructure/Order.Infrastructure.csproj Order.Infrastructure/
COPY src/Order.WebApi/Order.WebApi.csproj Order.WebApi/
RUN dotnet restore Order.WebApi/Order.WebApi.csproj

COPY src/Order.Core/ Order.Core/
COPY src/Order.Infrastructure/ Order.Infrastructure/
COPY src/Order.WebApi/ Order.WebApi/
RUN dotnet publish Order.WebApi/Order.WebApi.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Order.WebApi.dll"]
