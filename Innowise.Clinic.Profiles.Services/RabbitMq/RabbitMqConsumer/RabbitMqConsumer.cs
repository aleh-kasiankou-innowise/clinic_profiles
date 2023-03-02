using System.Text;
using System.Text.Json;
using Innowise.Clinic.Profiles.Dto.RabbitMq;
using Innowise.Clinic.Profiles.Exceptions.RabbitMq;
using Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;
using Innowise.Clinic.Profiles.Services.RabbitMq.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Innowise.Clinic.Profiles.Services.RabbitMq.RabbitMqConsumer;

public class RabbitMqConsumer : BackgroundService
{
    private readonly RabbitOptions _rabbitOptions;
    private readonly IConnection _connection;
    private readonly IModel _officeUpdateChannel;
    private readonly IModel _serviceUpdateChannel;
    private readonly IServiceProvider _services;

    public RabbitMqConsumer(IOptions<RabbitOptions> rabbitConfig, IServiceProvider services)
    {
        _services = services;
        _rabbitOptions = rabbitConfig.Value;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.HostName, UserName = _rabbitOptions.UserName, Password = _rabbitOptions.Password
        };
        _connection = factory.CreateConnection();
        _officeUpdateChannel = _connection.CreateModel();
        _serviceUpdateChannel = _connection.CreateModel();
    }

    public override void Dispose()
    {
        _officeUpdateChannel.Close();
        _serviceUpdateChannel.Close();
        _connection.Close();
        base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        SubscribeToSpecializationUpdateMessages();
        SubscribeToOfficeUpdateMessages();
    }

    private void SubscribeToOfficeUpdateMessages()
    {
        DeclareExchange(_rabbitOptions.OfficesProfilesExchangeName);
        var queue = CreateAndBindAnonymousQueue(_rabbitOptions.OfficeChangeRoutingKey,
            _rabbitOptions.OfficesProfilesExchangeName);
        var officeUpdatedConsumer = new EventingBasicConsumer(_officeUpdateChannel);
        officeUpdatedConsumer.Received += HandleOfficeUpdate;
        _officeUpdateChannel.BasicConsume(queue: queue,
            consumer: officeUpdatedConsumer);
    }

    private void HandleOfficeUpdate(object? model, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var officeChangeTask = JsonSerializer.Deserialize<OfficeChangeTask>(message) ??
                                   throw new DeserializationException(
                                       "The object received is not of OfficeChangeTask type.");
            using var scope = _services.CreateScope();
            var consistencyService = scope.ServiceProvider.GetRequiredService<IConsistencyService>();
            consistencyService.EnsureOfficeConsistency(officeChangeTask);
            _officeUpdateChannel.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void SubscribeToSpecializationUpdateMessages()
    {
        DeclareExchange(_rabbitOptions.ServicesProfilesExchangeName);
        var queue = CreateAndBindAnonymousQueue(_rabbitOptions.SpecializationChangeRoutingKey,
            _rabbitOptions.ServicesProfilesExchangeName);
        var specializationUpdatedConsumer = new EventingBasicConsumer(_serviceUpdateChannel);
        specializationUpdatedConsumer.Received += HandleSpecializationUpdate;
        _serviceUpdateChannel.BasicConsume(queue: queue,
            consumer: specializationUpdatedConsumer);
    }

    private void HandleSpecializationUpdate(object? model, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var specializationChangeTask = JsonSerializer.Deserialize<SpecializationChangeTaskDto>(message) ??
                                           throw new DeserializationException(
                                               "The object received is not of SpecializationChangeTaskDto type.");
            using var scope = _services.CreateScope();
            var consistencyService = scope.ServiceProvider.GetRequiredService<IConsistencyService>();
            consistencyService.EnsureSpecializationConsistency(specializationChangeTask);
            _serviceUpdateChannel.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void DeclareExchange(string exchangeName)
    {
        _officeUpdateChannel.ExchangeDeclare(exchange: exchangeName,
            type: ExchangeType.Topic);
    }

    private string CreateAndBindAnonymousQueue(string routingKey, string exchange)
    {
        var queueName = _officeUpdateChannel.QueueDeclare().QueueName;
        _officeUpdateChannel.QueueBind(queue: queueName,
            exchange: exchange,
            routingKey: routingKey);
        return queueName;
    }
}