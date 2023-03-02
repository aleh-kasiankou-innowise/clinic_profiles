namespace Innowise.Clinic.Profiles.Exceptions.ConsistencyManager;

public class UnsupportedTaskTypeException : ApplicationException
{
    private const string DefaultMessage = "The task type is not supported.";

    public UnsupportedTaskTypeException() : base(DefaultMessage)
    {
    }

    public UnsupportedTaskTypeException(string message) : base(message)
    {
    }
}