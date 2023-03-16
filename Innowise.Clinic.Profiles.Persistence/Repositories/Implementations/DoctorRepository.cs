using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Implementations;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDbContext _dbContext;

    public DoctorRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Doctor> CreateProfileAsync(Doctor doctor)
    {
        await _dbContext.Doctors.AddAsync(doctor);
        await _dbContext.SaveChangesAsync();
        return doctor;
    }

    public async Task<Doctor> GetProfileAsync(Guid doctorId)
    {
        // TODO TRY TO INCLUDE ENTITIES ONLY WHEN NEEDED
        var doctor = await _dbContext.Doctors
            .Include(x => x.Person)
            .Include(x => x.Specialization)
            .SingleOrDefaultAsync(x => x.DoctorId == doctorId);
        return doctor ??
               throw new ProfileNotFoundException("The doctor with the specified id is not registered in the system.");
    }

    public async Task<IEnumerable<Doctor>> GetDoctorListingAsync(int page, int quantity,
        Func<Doctor, bool>? specification = null)
    {
        // TODO TRY TO SELECT ONLY NECESSARY FIELDS
        // TODO IMPLEMENT SPECIFICATION PATTERN
        // USE-CASE - return only active doctors
        var doctorsQueryBase = specification is null
            ? _dbContext.Doctors.AsQueryable()
            : _dbContext.Doctors.Where(x => specification(x));

        return doctorsQueryBase.Include(x => x.Person)
            .Skip(CalculateOffset(page, quantity))
            .Take(quantity);
    }

    public async Task UpdateProfileAsync(Doctor updatedProfile)
    {
        _dbContext.Update(updatedProfile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<DoctorStatus> GetDoctorStatus(Guid doctorId)
    {
        return await _dbContext.Statuses.FindAsync(doctorId) ??
               throw new EntityNotFoundException($"There is no doctor status with id: {doctorId}");
    }
    
    private int CalculateOffset(int page, int pageSize) => page * pageSize - pageSize;
}