using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Implementations;

public class PatientRepository : IPatientRepository
{
    private readonly ProfilesDbContext _dbContext;

    public PatientRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Patient> CreateProfileAsync(Patient newProfile)
    {
        await _dbContext.Patients.AddAsync(newProfile);
        await _dbContext.SaveChangesAsync();
        return newProfile;
    }

    public async Task<Patient> GetPatientProfileAsync(Guid patientId)
    {
        return await _dbContext.Patients.Include(x => x.Person).FirstOrDefaultAsync(x => x.PersonId == patientId) ??
               throw new EntityNotFoundException($"The patient with id {patientId} does not exist.");
    }

    public async Task<IEnumerable<Patient>> GetPatientListingAsync(int page, int quantity)
    {
        return await _dbContext.Patients
            .Include(x => x.Person)
            .Skip(CalculateOffset(page, quantity))
            .Take(quantity)
            .ToListAsync();
    }

    public async Task UpdateProfileAsync(Patient updatedProfile)
    {
        _dbContext.Update(updatedProfile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProfileAsync(Guid patientId)
    {
        var patient = await _dbContext.Patients.SingleOrDefaultAsync(x => x.Person.PersonId == patientId) ??
                      throw new EntityNotFoundException($"The patient with id {patientId} does not exist.");
        _dbContext.Patients.Remove(patient);
        await _dbContext.SaveChangesAsync();
    }

    private int CalculateOffset(int page, int pageSize) => page * pageSize - pageSize;
}