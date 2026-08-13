using System.ComponentModel.DataAnnotations;

namespace EstoqueApi.Models;

public class Produto
{
    public int Id { get; set; }
    // O atributo [Required] indica que a propriedade é obrigatória, ou seja, não pode ser nula ou vazia.
    [Required]
    // O atributo [MaxLength(50)] define o tamanho máximo permitido para a propriedade, neste caso, 50 caracteres.
    [MaxLength(50)]
    // A propriedade Codigo representa o código do produto, que é uma string obrigatória com tamanho máximo de 50 caracteres e não pode ser vazia.
    public string Codigo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Saldo não pode ser negativo.")]
    public int Saldo { get; set; }
}
