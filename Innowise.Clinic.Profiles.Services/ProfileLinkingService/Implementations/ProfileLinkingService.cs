using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;
using Innowise.Clinic.Shared.Exceptions;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

namespace Innowise.Clinic.Profiles.Services.ProfileLinkingService.Implementations;

public class ProfileLinkingService : IProfileLinkingService
{
    private readonly ProfilesDbContext _dbContext;

    public ProfileLinkingService(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LinkAccountToProfile(UserProfileLinkingRequest profileLinkingDto)
    {
        var personProfile = _dbContext.Persons.FirstOrDefault(x => x.PersonId == profileLinkingDto.ProfileId) ??
                            throw new EntityNotFoundException(nameof(Person), nameof(Person.PersonId),
                                profileLinkingDto.ProfileId.ToString());

        personProfile.UserId = profileLinkingDto.UserId;
        _dbContext.Update(personProfile);
        await _dbContext.SaveChangesAsync();
    }
}