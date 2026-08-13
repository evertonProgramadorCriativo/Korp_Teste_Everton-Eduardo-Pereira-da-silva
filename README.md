# Korp_Teste_Everton Eduardo Pereira da silva

### Sistema de Emissão de Notas Fiscais

## korp Teste Everton Eduardo Pereira da silva

Projeto de desenvolvimento de um sistema para emissão e gerenciamento de Notas Fiscais, utilizando uma arquitetura baseada em serviços.

O sistema contempla:

- **Serviço de Estoque** gerenciamento de produtos, códigos, descrições e saldos.
- **Serviço de Faturamento**  gestão e emissão de notas fiscais.
- **API desenvolvida com .NET** e Entity Framework Core.
- **Banco de dados PostgreSQL**.
- Ambiente de desenvolvimento e execução utilizando **Docker e Docker Compose**.



## Objetivo

Implementar inicialmente o serviço de Estoque, permitindo:

- Criar produtos.
- Armazenar produtos em PostgreSQL.
- Consultar produtos.
- Consultar um produto pelo ID.
- Filtrar produtos pelo saldo mínimo.
- Validar dados de entrada.
- Impedir o cadastro de produtos com código duplicado.
- Executar a aplicação dentro de containers Docker.

## Tecnologias

- C#
- .NET
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker
- Docker Compose
- LINQ
- Swagger
- REST API


### Inicializar o projeto

```bash
docker compose up -d --build
```

### Verificar os containers

```bash
docker compose ps
```

### Testar a API

```bash
curl http://localhost:5001/
```

A API de Estoque fica disponível em:

http://localhost:5001

O Swagger pode ser acessado em:

http://localhost:5001/swagger

## Tabela Produto

Criação da tabela Produto

### Criar a migration

```bash
docker compose exec estoque-api dotnet ef migrations add CreateProduto
```

### Atualizar o banco

```bash
docker compose exec estoque-api dotnet ef database update
```

### Verificar as tabelas

```bash
docker compose exec postgres-estoque psql -U korp -d estoque -c "\dt"
```

### Verificar a estrutura da tabela

```bash
docker compose exec postgres-estoque psql -U korp -d estoque -c "\d produtos"
```

## Inserção de Produto


### Exemplo Linux

```bash
curl -X POST http://localhost:5001/api/produtos \
  -H "Content-Type: application/json" \
  -d '{"codigo": "P001", "descricao": "Caneta azul", "saldo": 10}'
```

### Exemplo no Windows 10 PowerShell

```bash
 $body = @{
    codigo = "P001"
    descricao = "Caneta azul"
    saldo = 10
} | ConvertTo-Json

Invoke-RestMethod `
    -Uri "http://localhost:5001/api/produtos" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```
 
### Exemplo no Windows 10 PowerShell com tratamento de erros


```bash
$body = @{
    quantidade = 23
} | ConvertTo-Json

try {
    Invoke-RestMethod `
        -Uri "http://localhost:5001/api/produtos/2/debitar" `
        -Method Post `
        -ContentType "application/json" `
        -Body $body
}
catch {
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    $responseBody = $reader.ReadToEnd()
    $reader.Close()

    Write-Host "Resposta da API:"
    Write-Host $responseBody
}
 
```

 
### Testando Tratarive de erro com produto ja existem no banco de dados


```bash
$body = @{
    quantidade = 23
} | ConvertTo-Json

try {
    Invoke-RestMethod `
        -Uri "http://localhost:5001/api/produtos/2/debitar" `
        -Method Post `
        -ContentType "application/json" `
        -Body $body
}
catch {
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    $responseBody = $reader.ReadToEnd()
    $reader.Close()

    Write-Host "Resposta da API:"
    Write-Host $responseBody
}


C:\Users\ogum\Documents\Korp_Teste_Everton_Eduardo> try {                              
>>     Invoke-RestMethod `
>>         -Uri "http://localhost:5001/api/produtos" `
>>         -ContentType "application/json" `
>>         -Body $body
>> }
>> catch {
>>     Write-Host "Status HTTP:" $_.Exception.Response.StatusCode.value__
>>     
>>     $reader = New-Object System.IO.StreamReader(
>>         $_.Exception.Response.GetResponseStream()
>>     )
>> 
>>     Write-Host "Mensagem da API:"
>>     Write-Host $reader.ReadToEnd()
>> 
>>     $reader.Close()
>> }
Status HTTP: 409
Mensagem da API:
{"mensagem":"Já existe um produto com o código 'P001'."}
PS C:\Users\ogum\Documents\Everton_Eduardo> 


```

### Verificar os registros no PostgreSQL

```bash
docker compose exec postgres-estoque psql -U korp -d estoque -c "SELECT * FROM produtos;"
```

### Validações

O cadastro possui validações para os dados enviados.

Código duplicado:

HTTP 409 — Conflict

Código ou descrição ausente:

HTTP 400 — Bad Request

Saldo negativo:

HTTP 400 — Bad Request

## Consulta de Produtos

O serviço disponibiliza endpoints para consultar produtos cadastrados.

### Listar produtos

GET /api/produtos

```bash
curl http://localhost:5001/api/produtos
```

### Consultar produto por ID

GET /api/produtos/{id}

```bash
curl http://localhost:5001/api/produtos/1
```

### Consultar produtos por saldo mínimo

GET /api/produtos?saldoMinimo=5

```bash
curl "http://localhost:5001/api/produtos?saldoMinimo=5"
```


## Execuntando codigo dentro do container do docker

```bash
docker compose exec estoque-api dotnet test
```

## Consulta dados no BD PostgreSQL:

```bash
docker compose exec postgres-estoque psql -U korp -d estoque -c "SELECT * FROM produtos;"
```

## Encerrar o ambiente

Para parar os containers:

```bash
docker compose down
```

Para parar os containers e remover também os volumes:

```bash
docker compose down -v
```

A opção `-v` remove os volumes associados ao PostgreSQL e, consequentemente, os dados armazenados no banco.
