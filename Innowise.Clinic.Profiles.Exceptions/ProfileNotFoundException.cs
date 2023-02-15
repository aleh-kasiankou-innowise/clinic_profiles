namespace Innowise.Clinic.Profiles.Exceptions;

public class ProfileNotFoundException : ApplicationException
{
    public ProfileNotFoundException(string message) : base(message)
    {
    }
}