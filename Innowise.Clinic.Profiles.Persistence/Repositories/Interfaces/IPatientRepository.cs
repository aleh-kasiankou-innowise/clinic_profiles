using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<Patient> CreateProfileAsync(Patient newProfile);
    Task<Patient> GetPatientProfileAsync(Guid patientId);
    Task<IEnumerable<Patient>> GetPatientListingAsync(int page, int quantity,
        Expression<Func<Patient, bool>> expression);
    Task UpdateProfileAsync(Patient updatedProfile);
    Task DeleteProfileAsync(Patient patient);
}