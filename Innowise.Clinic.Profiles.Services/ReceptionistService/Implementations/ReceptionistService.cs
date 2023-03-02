using System.Net.Http.Json;
using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Receptionist;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.Constants;
using Innowise.Clinic.Profiles.Services.RabbitMqPublisher;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Services.ReceptionistService.Implementations;

public class ReceptionistService : IReceptionistService
{
    private readonly ProfilesDbContext _dbContext;
    private readonly IRabbitMqPublisher _authenticationServiceConnection;

    public ReceptionistService(ProfilesDbContext dbContext,
        RabbitMqPublisher.RabbitMqPublisher authenticationServiceConnection)
    {
        _dbContext = dbContext;
        _authenticationServiceConnection = authenticationServiceConnection;
    }

    public async Task<Guid> CreateProfileAsync(CreateReceptionistProfileDto newProfile)
    {
        var newPerson = new Person
        {
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName,
            Photo = newProfile.Photo
        };

        var newReceptionist = new Receptionist
        {
            Email = newProfile.Email,
            OfficeId = newProfile.OfficeId,
            Person = newPerson
        };

        await _dbContext.Receptionists.AddAsync(newReceptionist);
        await _dbContext.SaveChangesAsync();

        var userCreationRequest =
            new AccountGenerationDto(newReceptionist.Person.PersonId, UserRoles.Receptionist, newReceptionist.Email);

        await new HttpClient().PostAsJsonAsync(ServicesRoutes.AccountGenerationEndpoint,
            userCreationRequest);

        return newReceptionist.Person.PersonId;
    }

    public async Task<ViewReceptionistProfileDto> GetProfileAsync(Guid receptionistId)
    {
        var receptionist = await GetReceptionistById(receptionistId);

        return new ViewReceptionistProfileDto
        {
            ReceptionistId = receptionist.Person.PersonId,
            FirstName = receptionist.Person.FirstName,
            LastName = receptionist.Person.LastName,
            MiddleName = receptionist.Person.MiddleName,
            OfficeId = receptionist.OfficeId,
            Photo = receptionist.Person.Photo
        };
    }

    public async Task<IEnumerable<ReceptionistInfoDto>> GetListingAsync()
    {
        var receptionistInfoDtos = _dbContext.Receptionists.Include(x => x.Person).Select(r => new ReceptionistInfoDto
        {
            ReceptionistId = r.Person.PersonId,
            FirstName = r.Person.FirstName,
            LastName = r.Person.LastName,
            MiddleName = r.Person.MiddleName,
            OfficeId = r.OfficeId
        });

        return await receptionistInfoDtos.ToListAsync();
    }

    public async Task UpdateProfileAsync(Guid receptionistId, EditReceptionistProfileDto updatedProfile)
    {
        var receptionist = await GetReceptionistById(receptionistId);

        receptionist.OfficeId = updatedProfile.OfficeId;
        receptionist.Person.FirstName = updatedProfile.FirstName;
        receptionist.Person.LastName = updatedProfile.LastName;
        receptionist.Person.MiddleName = updatedProfile.MiddleName;
        receptionist.Person.Photo = updatedProfile.Photo;

        _dbContext.Update(receptionist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProfileAsync(Guid receptionistId)
    {
        var receptionist = await GetReceptionistById(receptionistId);
        _authenticationServiceConnection.RemoveReceptionistAccount(receptionistId);
        _dbContext.Receptionists.Remove(receptionist);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Receptionist> GetReceptionistById(Guid id)
    {
        var receptionist = await _dbContext.Receptionists
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Person.PersonId == id);

        if (receptionist == null)
            throw new ProfileNotFoundException(
                "The receptionist with the requested id is not registered in the system.");

        return receptionist;
    }
}