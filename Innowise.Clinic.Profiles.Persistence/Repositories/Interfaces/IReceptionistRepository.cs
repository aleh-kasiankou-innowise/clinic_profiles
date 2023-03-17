using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

public interface IReceptionistRepository
{
    Task<Receptionist> CreateProfileAsync(Receptionist newProfile);
    // TODO USE EXPRESSION TREES TO DYNAMICALLY BUILD QUERIES
    Task<Receptionist> GetProfileAsync(Guid receptionistId);
    Task<IEnumerable<Receptionist>> GetListingAsync(int page, int quantity);
    Task<Receptionist> UpdateProfileAsync(Receptionist updatedProfile);
    Task DeleteProfileAsync(Guid receptionistId);
}