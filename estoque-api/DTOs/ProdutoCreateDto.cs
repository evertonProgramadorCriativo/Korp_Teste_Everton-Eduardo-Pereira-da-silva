// Definindo o modelo de dados para a criação de um produto.
using System.ComponentModel.DataAnnotations;

namespace EstoqueApi.DTOs;

public class ProdutoCreateDto
{  // required: indica que a propriedade é obrigatória e não pode ser nula.
    [Required]
    // MaxLength: define o tamanho máximo permitido para a propriedade.
    [MaxLength(50)]
    // O código do produto, que deve ser único e não nulo.
    public string Codigo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Saldo não pode ser negativo.")]
    public int Saldo { get; set; }
}
