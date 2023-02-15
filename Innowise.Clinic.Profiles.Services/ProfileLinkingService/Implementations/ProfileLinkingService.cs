using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;

namespace Innowise.Clinic.Profiles.Services.ProfileLinkingService.Implementations;

public class ProfileLinkingService : IProfileLinkingService
{
    private const string PatientRole = "Patient";
    private const string DoctorRole = "Doctor";
    private const string ReceptionistRole = "Receptionist";
    private readonly ProfilesDbContext _dbContext;

    public ProfileLinkingService(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LinkAccountToProfile(UserProfileLinkingDto profileLinkingDto)
    {
        var personProfile = _dbContext.Persons.FirstOrDefault(x => x.PersonId == profileLinkingDto.ProfileId);

        if (personProfile == null) throw new ProfileNotFoundException("There is no user with the requested id");

        personProfile.UserId = profileLinkingDto.UserId;
        _dbContext.Update(personProfile);
        await _dbContext.SaveChangesAsync();
    }
}