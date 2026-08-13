using EstoqueApi.Data;
using Microsoft.EntityFrameworkCore;

// builder configurado para rodar em ambiente de desenvolvimento
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o contexto do banco de dados para usar PostgreSQL, utilizando a string de conexão definida no arquivo de configuração (appsettings.json).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

builder.Services.AddDbContext<EstoqueDbContext>(options =>
    options.UseNpgsql(connectionString));

//Build é chamado para criar a aplicação web com as configurações definidas no builder. 
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Health check simples para confirmar que o serviço subiu
app.MapGet("/", () => Results.Ok(new { servico = "estoque-api", status = "online" }));

app.Run();
