using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Dto.RabbitMq;

namespace Innowise.Clinic.Profiles.Services.RabbitMqPublisher;

public interface IRabbitMqPublisher
{
    void RemoveReceptionistAccount(Guid accountId);
    void ChangeDoctorAccountStatus(AccountStatusChangeDto statusChangeDto);
    void SendAccountGenerationTask(AccountGenerationDto userCreationRequestDto);
}