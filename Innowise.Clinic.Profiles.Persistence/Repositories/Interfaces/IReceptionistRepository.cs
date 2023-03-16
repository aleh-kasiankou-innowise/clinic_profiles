using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

public interface IReceptionistRepository
{
    Task<Receptionist> CreateProfileAsync(Receptionist newProfile);
    Task<Receptionist> GetProfileAsync(Guid receptionistId);
    Task<IEnumerable<Receptionist>> GetListingAsync();
    Task<Receptionist> UpdateProfileAsync(Guid receptionistId, Receptionist updatedProfile);
    Task DeleteProfileAsync(Guid receptionistId);
}