FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Marco.AspNetCore.Cqs.WebApi/Marco.AspNetCore.Cqs.WebApi.csproj", "Marco.AspNetCore.Cqs.WebApi/"]
COPY ["Marco.AspNetCore.Cqs.Application/Marco.AspNetCore.Cqs.Application.csproj", "Marco.AspNetCore.Cqs.Application/"]
COPY ["Marco.AspNetCore.Cqs.Domain/Marco.AspNetCore.Cqs.Domain.csproj", "Marco.AspNetCore.Cqs.Domain/"]
COPY ["Marco.AspNetCore.Cqs.Infra.Data.Dapper/Marco.AspNetCore.Cqs.Infra.Data.Dapper.csproj", "Marco.AspNetCore.Cqs.Infra.Data.Dapper/"]
RUN dotnet restore "Marco.AspNetCore.Cqs.WebApi/Marco.AspNetCore.Cqs.WebApi.csproj"

COPY . .
WORKDIR "/src/Marco.AspNetCore.Cqs.WebApi"
RUN dotnet publish "Marco.AspNetCore.Cqs.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Marco.AspNetCore.Cqs.WebApi.dll"]
