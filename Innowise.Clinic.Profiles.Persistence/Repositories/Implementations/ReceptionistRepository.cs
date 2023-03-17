using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Persistence.Utilities;
using Innowise.Clinic.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Implementations;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfilesDbContext _dbContext;

    public ReceptionistRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Receptionist> CreateProfileAsync(Receptionist newProfile)
    {
        await _dbContext.Receptionists.AddAsync(newProfile);
        await _dbContext.SaveChangesAsync();
        return newProfile;
    }

    public async Task<Receptionist> GetProfileAsync(Guid receptionistId)
    {
        var receptionist = await _dbContext.Receptionists
                               .Include(x => x.Person)
                               .FirstOrDefaultAsync(x => x.Person.PersonId == receptionistId) ??
                           throw new EntityNotFoundException(nameof(Receptionist), nameof(Receptionist.PersonId),
                               receptionistId.ToString());

        return receptionist;
    }

    public async Task<IEnumerable<Receptionist>> GetListingAsync(int page, int quantity)
    {
        return await _dbContext.Receptionists
            .Include(x => x.Person)
            .Skip(RepositoryUtilities.CalculateOffset(page, quantity))
            .Take(quantity)
            .ToListAsync();
    }

    public async Task<Receptionist> UpdateProfileAsync(Receptionist updatedProfile)
    {
        _dbContext.Update(updatedProfile);
        await _dbContext.SaveChangesAsync();
        return updatedProfile;
    }

    public async Task DeleteProfileAsync(Guid receptionistId)
    {
        var receptionist = await _dbContext.Receptionists.FindAsync(receptionistId) ??
                           throw new EntityNotFoundException(nameof(Receptionist), nameof(Receptionist.PersonId),
                               receptionistId.ToString());

        _dbContext.Receptionists.Remove(receptionist);
        await _dbContext.SaveChangesAsync();
    }
}