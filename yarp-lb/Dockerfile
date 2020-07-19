ARG SDK_VERSION=3.1

FROM mcr.microsoft.com/dotnet/core/sdk:${SDK_VERSION} AS build

WORKDIR /source

COPY src/*.csproj .

RUN dotnet restore -r linux-musl-x64

COPY src/ .

RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:${SDK_VERSION}-alpine

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["./YarpProxy"]