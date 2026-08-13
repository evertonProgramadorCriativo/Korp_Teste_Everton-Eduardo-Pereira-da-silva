// Definindo o modelo de dados para a resposta de um produto.
namespace EstoqueApi.DTOs;

public class ProdutoResponseDto
{
    public int Id { get; set; }
    // O código do produto, que deve ser único e não nulo.
    //get e set são propriedades de leitura e escrita, respectivamente.
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Saldo { get; set; }
}
