using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Persistence.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class ConsistencyCheckRequestConsumer : IConsumer<ProfileExistsAndHasRoleRequest>
{
    private readonly ProfilesDbContext _dbContext;

    public ConsistencyCheckRequestConsumer(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProfileExistsAndHasRoleRequest> context)
    {
        var profile = await _dbContext.Patients.FindAsync(context.Message.ProfileId.ToString());

        if (profile is not null)
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

            if (await dbContext.AnyAsync(x => x.PersonId == context.Message.ProfileId))
            {
                await context.RespondAsync(
                    new ProfileExistsAndHasRoleResponse(false,
                        $"The profile exists but doesn't belong to a {context.Message.Role.ToLower()} group."));
            }

            else
            {
                await context.RespondAsync(
                    new ServiceExistsAndBelongsToSpecializationResponse(true, null));
            }
        }

        else
        {
            await context.RespondAsync(
                new ServiceExistsAndBelongsToSpecializationResponse(false, "The requested profile does not exist."));
        }
    }
}