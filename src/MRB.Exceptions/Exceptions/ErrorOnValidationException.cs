namespace MRB.Exceptions.Exceptions;

public class ErrorOnValidationException : MyRecipesBookExceptionBase
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> erroMessages) : base(string.Empty)
    {
        ErrorMessages = erroMessages;
    }
}