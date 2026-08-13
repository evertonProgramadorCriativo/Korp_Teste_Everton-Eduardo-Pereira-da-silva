namespace EstoqueApi.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string mensagem) : base(StatusCodes.Status404NotFound, mensagem)
    {
    }
}
