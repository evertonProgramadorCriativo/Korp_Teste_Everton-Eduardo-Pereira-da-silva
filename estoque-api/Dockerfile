FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

# Ferramenta dotnet-ef instalada dentro da imagem, necessária para
# rodar "dotnet ef migrations add" / "dotnet ef database update"
# de dentro do container (docker compose exec estoque-api dotnet ef ...)
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="$PATH:/root/.dotnet/tools"

COPY *.csproj ./
RUN dotnet restore

COPY . .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

CMD ["dotnet", "run", "--no-launch-profile", "--urls", "http://+:8080"]
