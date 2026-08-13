namespace EstoqueApi.Exceptions;

// Herda de ConflictException (409): a operação conflita com o estado
// atual do saldo do produto - não é um erro de validação de formato
// (400), é uma regra de negócio sendo violada.
public class SaldoInsuficienteException : ConflictException
{
    public SaldoInsuficienteException(string mensagem) : base(mensagem)
    {
    }
}
