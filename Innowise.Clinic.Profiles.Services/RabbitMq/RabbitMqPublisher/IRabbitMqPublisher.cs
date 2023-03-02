using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Dto.RabbitMq;

namespace Innowise.Clinic.Profiles.Services.RabbitMq.RabbitMqPublisher;

public interface IRabbitMqPublisher
{
    void RemoveReceptionistAccount(Guid accountId);
    void ChangeDoctorAccountStatus(AccountStatusChangeDto statusChangeDto);
    void SendAccountGenerationTask(AccountGenerationDto userCreationRequestDto);
}