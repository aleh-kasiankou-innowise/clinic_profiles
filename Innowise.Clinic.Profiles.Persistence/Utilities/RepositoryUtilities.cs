namespace Innowise.Clinic.Profiles.Persistence.Utilities;

public static class RepositoryUtilities
{
    public static int CalculateOffset(int page, int pageSize) => page * pageSize - pageSize;
}