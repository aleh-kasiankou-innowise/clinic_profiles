using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class ProfileExistsAndHasRoleRequestConsumer : IConsumer<ProfileExistsAndHasRoleRequest>
{
    private readonly ProfilesDbContext _dbContext;
    private readonly ILogger<ProfileExistsAndHasRoleRequestConsumer> _logger;

    public ProfileExistsAndHasRoleRequestConsumer(ProfilesDbContext dbContext, ILogger<ProfileExistsAndHasRoleRequestConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProfileExistsAndHasRoleRequest> context)
    {
        try
        {
            IQueryable<IPersonRelatable> dbContext;

            switch (context.Message.Role)
            {
                case UserRoles.Patient:
                    dbContext = _dbContext.Patients.AsQueryable();
                    break;
                case UserRoles.Doctor:
                    dbContext = _dbContext.Doctors.AsQueryable();
                    break;
                case UserRoles.Receptionist:
                    dbContext = _dbContext.Receptionists.AsQueryable();
                    break;
                default:
                    throw new InvalidInputDataException($"The unknown role : {context.Message.Role}");
            }

            if (!await dbContext.AnyAsync(x => x.PersonId == context.Message.ProfileId))
            {
                var response = new ProfileExistsAndHasRoleResponse(false,
                    $"The profile doesn't belong to a {context.Message.Role.ToLower()} group.");
                await context.RespondAsync(response);
                _logger.LogInformation("Person with profile id {Id} doesn't belong to a {Group} group",
                    context.Message.ProfileId, context.Message.Role);
            }

            else
            {
                var response = new ProfileExistsAndHasRoleResponse(true, null);
                await context.RespondAsync(response);
                _logger.LogInformation("Person with profile id {Id} exists and belongs to a {Group} group",
                    context.Message.ProfileId, context.Message.Role);
            }
        }

        finally
        {
            _logger.LogInformation("Consistency check for doctor with id {DoctorId} is finished",
                context.Message.ProfileId);
        }
    }
}