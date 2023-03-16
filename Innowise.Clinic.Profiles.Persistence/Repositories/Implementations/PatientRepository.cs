using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Implementations;

public class PatientRepository : IPatientRepository
{
    public async Task<Patient> CreateProfileAsync(Patient newProfile)
    {
        throw new NotImplementedException();
    }

    public async Task<Patient> GetPatientProfileAsync(Guid patientId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Patient>> GetPatientListingAsync()
    {
        throw new NotImplementedException();
    }

    public async Task UpdateProfileAsync(Guid patientId, Patient updatedProfile)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteProfileAsync(Guid patientId)
    {
        throw new NotImplementedException();
    }
}