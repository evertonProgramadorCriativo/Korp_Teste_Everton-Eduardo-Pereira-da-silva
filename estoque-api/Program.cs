using EstoqueApi.Data;
using Microsoft.EntityFrameworkCore;

// Cria o builder da aplicação e carrega as configurações
// do ambiente e do arquivo appsettings.json.
var builder = WebApplication.CreateBuilder(args);

// Registra os Controllers responsáveis pelos endpoints da API.
builder.Services.AddControllers();

// Habilita a geração dos metadados necessários para o Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o CORS para permitir que o frontend Angular,
// executado em http://localhost:4200, consuma a API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Obtém a string de conexão configurada no appsettings.json.
// Caso não esteja configurada, interrompe a inicialização da aplicação.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' não configurada."
    );

// Configura o Entity Framework Core para utilizar PostgreSQL
// como banco de dados da aplicação.
builder.Services.AddDbContext<EstoqueDbContext>(options =>
    options.UseNpgsql(connectionString));

// Constrói a aplicação com todos os serviços e configurações registradas.
var app = builder.Build();

// Habilita o Swagger para documentação e testes da API.
app.UseSwagger();
app.UseSwaggerUI();

// Habilita a política de CORS para permitir requisições
// provenientes do frontend Angular.
app.UseCors("Frontend");

// Mapeia os Controllers da aplicação.
app.MapControllers();

// Health Check simples para confirmar que o serviço está online.
app.MapGet("/", () => Results.Ok(new
{
    servico = "estoque-api",
    status = "online"
}));

// Inicia a aplicação web.
app.Run();