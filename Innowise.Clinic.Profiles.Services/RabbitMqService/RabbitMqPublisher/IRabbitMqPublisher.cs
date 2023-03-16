using Innowise.Clinic.Profiles.Dto.RabbitMq;
using Innowise.Clinic.Shared.Dto;

namespace Innowise.Clinic.Profiles.Services.RabbitMqService.RabbitMqPublisher;

public interface IRabbitMqPublisher
{
    void RemoveReceptionistAccount(Guid accountId);
    void ChangeDoctorAccountStatus(AccountStatusChangeDto statusChangeDto);
    void SendAccountGenerationTask(AccountGenerationDto userCreationRequestDto);
}