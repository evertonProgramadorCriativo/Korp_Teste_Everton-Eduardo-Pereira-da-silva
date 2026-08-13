
using EstoqueApi.Data;
using EstoqueApi.DTOs;
using EstoqueApi.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstoqueApi.Controllers;

//Informa ao ASP.NET Core que esta classe será utilizada como um Controller de uma API [ApiController]
[ApiController]

//Este Controller é responsável por receber as requisições HTTP relacionadas aos produtos e executar as operações necessárias no banco de dados.
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    //CONTEXTO DO BANCO DE DADOS

    private readonly EstoqueDbContext _context;

    public ProdutosController(EstoqueDbContext context)
    {
        //_context representa o EstoqueDbContext. Ele é usado para interagir com o banco de dados, permitindo realizar operações como consultas, inserções, atualizações e exclusões de produtos.
        _context = context;
    }

    //  Inserção de Produto
    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Criar([FromBody] ProdutoCreateDto dto)
    {  // Validação do modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Verifica se já existe um produto com o mesmo código
        var codigoJaExiste = await _context.Produtos
            .AnyAsync(p => p.Codigo == dto.Codigo);

        if (codigoJaExiste)
        {
            throw new ConflictException($"Já existe um produto com o código '{dto.Codigo}'.");
        }
        // Cria o produto
        var produto = new Produto
        {
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            Saldo = dto.Saldo
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var response = ParaDto(produto);

        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, response);
    }

    // Consulta de produtos 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> Listar([FromQuery] int? saldoMinimo)
    {
        IQueryable<Produto> query = _context.Produtos;

        if (saldoMinimo.HasValue)
        {
            query = query.Where(p => p.Saldo >= saldoMinimo.Value);
        }

        var produtos = await query
            .OrderBy(p => p.Codigo)
            .ToListAsync();

        return Ok(produtos.Select(ParaDto));
    }

    // Consulta produto por id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> ObterPorId(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
        {
            return NotFound(new { mensagem = $"Produto com id {id} não encontrado." });
        }

        return Ok(ParaDto(produto));
    }
    // Débito de saldo (usado, por exemplo, na impressão da
    // nota fiscal). Regra: nunca deixa o saldo ficar negativo.
    [HttpPost("{id:int}/debitar")]
    public async Task<ActionResult<ProdutoResponseDto>> Debitar(int id, [FromBody] AtualizarSaldoDto dto)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return BadRequest($"Produto com id {id} não encontrado.");
        }

        if (dto.Quantidade > produto.Saldo)
        {
            return BadRequest(
                $"Saldo insuficiente para o produto '{produto.Codigo}'. " +
                $"Saldo atual: {produto.Saldo}, quantidade solicitada: {dto.Quantidade}."
            );
        }


        produto.Saldo -= dto.Quantidade;
        await _context.SaveChangesAsync();

        return Ok(ParaDto(produto));
    }
    // Método auxiliar para converter Produto em ProdutoResponseDto
    private static ProdutoResponseDto ParaDto(Produto produto) => new()
    {
        Id = produto.Id,
        Codigo = produto.Codigo,
        Descricao = produto.Descricao,
        Saldo = produto.Saldo
    };
}
