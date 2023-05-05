using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class DoctorNameRequestConsumer : IConsumer<DoctorNameRequest>
{
    private readonly IDoctorService _doctorService;

    public DoctorNameRequestConsumer(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    public async Task Consume(ConsumeContext<DoctorNameRequest> context)
    {
        var doctorInfo = await _doctorService.GetProfileAsync(context.Message.DoctorId);
        var fullName = doctorInfo.MiddleName is null
            ? $"{doctorInfo.FirstName} {doctorInfo.LastName}"
            : $"{doctorInfo.FirstName} {doctorInfo.MiddleName} {doctorInfo.LastName}";
        await context.RespondAsync<DoctorNameResponse>(new(fullName));
    }
}