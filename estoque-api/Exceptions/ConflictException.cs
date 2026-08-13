namespace EstoqueApi.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string mensagem) : base(StatusCodes.Status409Conflict, mensagem)
    {
    }
}
