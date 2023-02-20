using System.Net.Http.Json;
using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Services.PatientService.Implementations;

public class PatientService : IPatientService
{
    private readonly ProfilesDbContext _dbContext;

    public PatientService(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Guid> CreateProfileAsync(PatientProfileDto newProfile)
    {
        var newPerson = new Person
        {
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName
        };

        var newPatientProfile = new Patient
        {
            Person = newPerson,
            DateOfBirth = newProfile.DateOfBirth
        };

        await _dbContext.Patients.AddAsync(newPatientProfile);
        await _dbContext.SaveChangesAsync();

        return newPatientProfile.Person.PersonId;
    }

    public async Task<Guid> CreateProfileAsync(PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId)
    {
        var newPerson = new Person
        {
            Photo = newProfile.Photo,
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName,
            UserId = associatedUserId
        };

        var newPatientProfile = new Patient
        {
            Person = newPerson,
            PhoneNumber = newProfile.PhoneNumber,
            DateOfBirth = newProfile.DateOfBirth
        };

        await _dbContext.Patients.AddAsync(newPatientProfile);
        await _dbContext.SaveChangesAsync();

        await new HttpClient().PostAsJsonAsync("http://auth:80/helperservices/force-log-out", associatedUserId);
        await new HttpClient().PostAsJsonAsync("http://auth:80/helperservices/link-to-profile",
            new UserProfileLinkingDto(associatedUserId, newPatientProfile.Person.PersonId));

        return newPatientProfile.Person.PersonId;
    }

    public async Task<ViewPatientProfileDto> GetPatientProfileAsync(Guid patientId)
    {
        var patient = await FindPatientById(patientId);

        var patientData = new ViewPatientProfileDto
        {
            PatientId = patient.Person.PersonId,
            FirstName = patient.Person.FirstName,
            LastName = patient.Person.LastName,
            MiddleName = patient.Person.MiddleName,
            DateOfBirth = patient.DateOfBirth,
            PhoneNumber = patient.PhoneNumber,
            Photo = patient.Person.Photo
        };

        return patientData;
    }

    public async Task<IEnumerable<PatientInfoDto>> GetPatientListingAsync()
    {
        var patientInfoDtos = _dbContext.Patients
            .Include(p => p.Person)
            .Select(x => new PatientInfoDto
            {
                PatientId = x.Person.PersonId,
                FirstName = x.Person.FirstName,
                LastName = x.Person.LastName,
                MiddleName = x.Person.MiddleName,
                PhoneNumber = x.PhoneNumber
            });

        return await patientInfoDtos.ToListAsync();
    }

    public async Task UpdateProfileAsync(Guid patientId, PatientProfileWithNumberAndPhotoDto updatedProfile)
    {
        var patient = await FindPatientById(patientId);

        patient.Person.FirstName = updatedProfile.FirstName;
        patient.Person.LastName = updatedProfile.LastName;
        patient.Person.MiddleName = updatedProfile.MiddleName;
        patient.Person.Photo = updatedProfile.Photo ?? patient.Person.Photo;
        patient.DateOfBirth = updatedProfile.DateOfBirth;
        patient.PhoneNumber = updatedProfile.PhoneNumber;

        _dbContext.Update(patient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProfileAsync(Guid patientId)
    {
        var patient = await FindPatientById(patientId);

        _dbContext.Patients.Remove(patient);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Patient> FindPatientById(Guid id)
    {
        var patient = await _dbContext.Patients.Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Person.PersonId == id);

        if (patient == null)
            throw new ProfileNotFoundException("The patient with the requested id is not registered in the system.");

        return patient;
    }
}