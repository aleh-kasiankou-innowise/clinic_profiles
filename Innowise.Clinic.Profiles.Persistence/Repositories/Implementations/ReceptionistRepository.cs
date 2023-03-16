using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Implementations;

public class ReceptionistRepository : IReceptionistRepository
{
    public async Task<Receptionist> CreateProfileAsync(Receptionist newProfile)
    {
        throw new NotImplementedException();
    }

    public async Task<Receptionist> GetProfileAsync(Guid receptionistId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Receptionist>> GetListingAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Receptionist> UpdateProfileAsync(Guid receptionistId, Receptionist updatedProfile)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteProfileAsync(Guid receptionistId)
    {
        throw new NotImplementedException();
    }
}