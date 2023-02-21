using System.Net.Http.Json;
using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.Constants;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Services.DoctorService.Implementations;

public class DoctorService : IDoctorService
{
    private readonly ProfilesDbContext _dbContext;

    public DoctorService(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateProfileAsync(CreateEditDoctorProfileDto newProfile)
    {
        var httpClient = new HttpClient();
        // ensure office, specialization and status are valid
        var officeConsistencyCheck = await httpClient.GetAsync($"http://office:80/helperservices/ensure-exists/office/{newProfile.OfficeId}");
        if (!officeConsistencyCheck.IsSuccessStatusCode) throw new NotImplementedException();
        
        var specializationConsistencyCheck =
            await httpClient.GetAsync($"http://service:80/helperservices/ensure-exists/specialization/{newProfile.SpecializationId}");
        if (!specializationConsistencyCheck.IsSuccessStatusCode) throw new NotImplementedException();

        var newPerson = new Person
        {
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName,
            Photo = newProfile.Photo
        };

        var newDoctor = new Doctor
        {
            Person = newPerson,
            CareerStartYear = newProfile.CareerStartYear,
            Email = newProfile.Email,
            OfficeId = newProfile.OfficeId,
            SpecializationId = newProfile.SpecializationId,
            StatusId = newProfile.StatusId
        };

        await _dbContext.Doctors.AddAsync(newDoctor);
        await _dbContext.SaveChangesAsync();

        var userCreationRequest =
            new UserCreationRequestDto(newDoctor.Person.PersonId, UserRoles.Doctor, newDoctor.Email);

        await new HttpClient().PostAsJsonAsync(ServicesRoutes.AccountGenerationEndpoint, userCreationRequest);

        return newDoctor.Person.PersonId;
    }

    public async Task<ViewDoctorProfileDto> GetProfileAsync(Guid doctorId)
    {
        var doctor = await FindDoctorById(doctorId);

        var doctorData = new ViewDoctorProfileDto
        {
            DoctorId = doctor.Person.PersonId,
            CareerStartYear = doctor.CareerStartYear,
            StatusId = doctor.StatusId,
            DateOfBirth = doctor.DateOfBirth,
            Email = doctor.Email,
            FirstName = doctor.Person.FirstName,
            LastName = doctor.Person.LastName,
            MiddleName = doctor.Person.MiddleName,
            OfficeId = doctor.OfficeId,
            SpecializationId = doctor.SpecializationId,
            Photo = doctor.Person.Photo
        };

        return doctorData;
    }

    public async Task<IEnumerable<DoctorInfoDto>> GetListingAsync()
    {
        var doctorInfoDtos = _dbContext.Doctors.Include(x => x.Person).Select(d => new DoctorInfoDto(d.DoctorId,
            d.Person.FirstName,
            d.Person.LastName, d.Person.MiddleName, d.SpecializationId, d.OfficeId, d.CareerStartYear, d.Person.Photo));

        return await doctorInfoDtos.ToListAsync();
    }

    public async Task<DoctorInfoDto> GetPublicInfo(Guid id)
    {
        var doctor = await FindDoctorById(id);

        return new DoctorInfoDto(doctor.DoctorId,
            doctor.Person.FirstName,
            doctor.Person.LastName, doctor.Person.MiddleName, doctor.SpecializationId, doctor.OfficeId,
            doctor.CareerStartYear, doctor.Person.Photo);
    }

    public async Task<IEnumerable<DoctorInfoReceptionistDto>> GetListingForReceptionistAsync()
    {
        var doctorInfoReceptionistDtos = _dbContext.Doctors.Include(x => x.Person).Select(d =>
            new DoctorInfoReceptionistDto(d.DoctorId, d.Person.FirstName, d.Person.LastName, d.Person.MiddleName,
                d.SpecializationId, d.OfficeId, d.StatusId));

        return await doctorInfoReceptionistDtos.ToListAsync();
    }

    public async Task UpdateProfileAsync(Guid doctorId, CreateEditDoctorProfileDto updatedProfile)
    {
        var doctor = await FindDoctorById(doctorId);

        doctor.Person.FirstName = updatedProfile.FirstName;
        doctor.Person.LastName = updatedProfile.LastName;
        doctor.Person.MiddleName = updatedProfile.MiddleName;
        doctor.DateOfBirth = updatedProfile.DateOfBirth;
        doctor.Person.Photo = updatedProfile.Photo;
        doctor.Email = updatedProfile.Email;
        doctor.StatusId = updatedProfile.StatusId;
        doctor.SpecializationId = updatedProfile.SpecializationId;
        doctor.CareerStartYear = updatedProfile.CareerStartYear;

        _dbContext.Update(doctor);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Guid doctorId, Guid newStatusId)
    {
        var doctor = await FindDoctorById(doctorId);
        _dbContext.Remove(doctor);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Doctor> FindDoctorById(Guid doctorId)
    {
        var doctor = await _dbContext.Doctors
            .Include(x => x.Person)
            .FirstOrDefaultAsync(x => x.Person.PersonId == doctorId);

        if (doctor == null)
            throw new ProfileNotFoundException("The doctor with the specified id is not registered in the system.");

        return doctor;
    }
}