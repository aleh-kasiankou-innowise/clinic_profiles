using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Persistence.Utilities;
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
            .Include(x => x.Status)
            .SingleOrDefaultAsync(x => x.Person.PersonId == doctorId);
        return doctor ??
               throw new EntityNotFoundException(nameof(Doctor), nameof(Doctor.PersonId),
                   doctorId.ToString());
    }

    public async Task<IEnumerable<Doctor>> GetDoctorListingAsync(int page, int quantity,
        Expression<Func<Doctor, bool>>? filter = null)
    {
        var doctorsQueryBase = filter is null
            ? _dbContext.Doctors.AsQueryable()
            : _dbContext.Doctors.Include(x => x.Status)
                .Where(filter);

        return doctorsQueryBase.Include(x => x.Person)
            .Skip(RepositoryUtilities.CalculateOffset(page, quantity))
            .Take(quantity);
    }

    public async Task UpdateProfileAsync(Doctor updatedProfile)
    {
        _dbContext.Update(updatedProfile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<DoctorStatus> GetDoctorStatus(Guid doctorStatusId)
    {
        return await _dbContext.Statuses.FindAsync(doctorStatusId) ??
               throw new EntityNotFoundException(nameof(DoctorStatus), nameof(DoctorStatus.StatusId),
                   doctorStatusId.ToString());
    }
}