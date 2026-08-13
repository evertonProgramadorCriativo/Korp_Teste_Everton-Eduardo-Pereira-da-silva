using System.ComponentModel.DataAnnotations;

namespace EstoqueApi.DTOs;

public class AtualizarSaldoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}
