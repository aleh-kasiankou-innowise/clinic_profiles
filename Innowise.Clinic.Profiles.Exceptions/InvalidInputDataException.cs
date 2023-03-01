namespace Innowise.Clinic.Profiles.Exceptions;

public class InvalidInputDataException : ApplicationException
{
    private const string DefaultMessage = "The input data is not correctly formatted. " +
                                    "Please check the swagger examples to get the list of acceptable data types.";
    public InvalidInputDataException(string message) : base(message)
    {
        
    }

    public InvalidInputDataException() : base(DefaultMessage)
    {
    }
}