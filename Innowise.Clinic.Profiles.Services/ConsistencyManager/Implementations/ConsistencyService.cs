using Innowise.Clinic.Profiles.Dto.RabbitMq;
using Innowise.Clinic.Profiles.Exceptions.ConsistencyManager;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;

namespace Innowise.Clinic.Profiles.Services.ConsistencyManager.Implementations;

public class ConsistencyService : IConsistencyService
{
    private readonly ProfilesDbContext _dbContext;

    public ConsistencyService(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void EnsureOfficeConsistency(OfficeChangeTask officeChangeTask)
    {
        switch (officeChangeTask.TaskType)
        {
            case OfficeChange.Add:
            case OfficeChange.Update:
                CreateOrUpdateOffice(officeChangeTask.OfficeAddress);
                break;
            case OfficeChange.Remove:
                DeleteOfficeIfExists(officeChangeTask.OfficeAddress);
                break;

            default:
                throw new UnsupportedTaskTypeException(
                    $"Unsupported office change task type: {officeChangeTask.TaskType}");
        }
    }

    public void EnsureSpecializationConsistency(SpecializationChangeTaskDto specializationChangeTask)
    {
        switch (specializationChangeTask.TaskType)
        {
            case SpecializationChange.Add:
            case SpecializationChange.Update:
                CreateOrUpdateSpecialization(specializationChangeTask.SpecializationDto);
                break;

            default:
                throw new UnsupportedTaskTypeException(
                    $"Unsupported specialization change task type: {specializationChangeTask.TaskType}");
        }
    }

    private void CreateOrUpdateOffice(OfficeAddressDto officeAddressDto)
    {
        if (officeAddressDto.Address is null)
        {
            throw new MissingDataException("Office address cannot be null.");
        }

        var existenceCheck = TryGetOffice(officeAddressDto.OfficeId);
        if (existenceCheck is { OfficeExists: true, Office: { } })
        {
            var office = existenceCheck.Office;
            office.OfficeAddress = officeAddressDto.Address;
            _dbContext.Update(office);
            _dbContext.SaveChanges();
            return;
        }

        var newOffice = new Office()
        {
            OfficeId = officeAddressDto.OfficeId,
            OfficeAddress = officeAddressDto.Address
        };
        _dbContext.Offices.Add(newOffice);
        _dbContext.SaveChanges();
    }


    private void DeleteOfficeIfExists(OfficeAddressDto officeAddressDto)
    {
        var existenceCheck = TryGetOffice(officeAddressDto.OfficeId);
        if (existenceCheck is { OfficeExists: true, Office: { } })
        {
            _dbContext.Remove(existenceCheck.Office);
            _dbContext.SaveChanges();
        }
    }

    private void CreateOrUpdateSpecialization(SpecializationDto specializationDto)
    {
        var existenceCheck = TryGetSpecialization(specializationDto.SpecializationId);
        if (existenceCheck is { SpecializationExists: true, Specialization: { } })
        {
            var specialization = existenceCheck.Specialization;
            specialization.SpecializationName = specializationDto.SpecializationName;
            _dbContext.Update(specialization);
            _dbContext.SaveChanges();
            return;
        }

        var newSpecialization = new Specialization()
        {
            SpecializationId = specializationDto.SpecializationId,
            SpecializationName = specializationDto.SpecializationName
        };
        _dbContext.Specializations.Add(newSpecialization);
        _dbContext.SaveChanges();
    }

    private (bool OfficeExists, Office? Office) TryGetOffice(Guid officeId)
    {
        var office = _dbContext.Offices.FirstOrDefault(x => x.OfficeId == officeId);
        return (office is not null, office);
    }

    private (bool SpecializationExists, Specialization? Specialization) TryGetSpecialization(Guid specializationId)
    {
        var specialization = _dbContext.Specializations.FirstOrDefault(x => x.SpecializationId == specializationId);
        return (specialization is not null, specialization);
    }
}