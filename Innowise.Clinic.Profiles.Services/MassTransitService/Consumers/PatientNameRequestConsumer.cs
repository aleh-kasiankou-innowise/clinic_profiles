using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class PatientNameRequestConsumer : IConsumer<PatientNameRequest>
{
    private readonly IPatientService _patientService;

    public PatientNameRequestConsumer(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public async Task Consume(ConsumeContext<PatientNameRequest> context)
    {
        var patient = await _patientService.GetPatientProfileAsync(context.Message.PatientId);
        var patientFullName = patient.MiddleName is null
            ? $"{patient.FirstName} {patient.LastName}"
            : $"{patient.FirstName} {patient.MiddleName} {patient.LastName}";
        await context.RespondAsync<PatientNameResponse>(new(patientFullName));
    }
}