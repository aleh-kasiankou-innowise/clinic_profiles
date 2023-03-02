using System.Text.Json;
using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Dto.RabbitMq;
using Innowise.Clinic.Profiles.Services.RabbitMq.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Innowise.Clinic.Profiles.Services.RabbitMq.RabbitMqPublisher;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IModel _channel;
    private readonly RabbitOptions _rabbitOptions;

    public RabbitMqPublisher(IOptions<RabbitOptions> rabbitConfig)
    {
        _rabbitOptions = rabbitConfig.Value;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.HostName, UserName = _rabbitOptions.UserName, Password = _rabbitOptions.Password
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        DeclareProfileAuthenticationExchange();
    }

    public void RemoveReceptionistAccount(Guid accountId)
    {
        var body = accountId.ToByteArray();
        _channel.BasicPublish(exchange: _rabbitOptions.ProfilesAuthenticationExchangeName,
            routingKey: _rabbitOptions.ReceptionistRemovedRoutingKey,
            basicProperties: null,
            body: body);
    }

    public void ChangeDoctorAccountStatus(AccountStatusChangeDto statusChangeDto)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(statusChangeDto);
        _channel.BasicPublish(exchange: _rabbitOptions.ProfilesAuthenticationExchangeName,
            routingKey: _rabbitOptions.DoctorInactiveRoutingKey,
            basicProperties: null,
            body: body);
    }

    public void SendAccountGenerationTask(AccountGenerationDto userCreationRequestDto)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(userCreationRequestDto);
        _channel.BasicPublish(exchange: _rabbitOptions.ProfilesAuthenticationExchangeName,
            routingKey: _rabbitOptions.AccountGenerationRoutingKey,
            basicProperties: null,
            body: body);
    }

    private void DeclareProfileAuthenticationExchange()
    {
        _channel.ExchangeDeclare(exchange: _rabbitOptions.ProfilesAuthenticationExchangeName,
            type: ExchangeType.Topic);
    }
}