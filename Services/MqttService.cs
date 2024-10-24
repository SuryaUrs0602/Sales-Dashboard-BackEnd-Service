﻿
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using SalesDashBoardApplication.Models;
using System.Linq.Expressions;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SalesDashBoardApplication.Services
{
    public class MqttService : IHostedService
    {

        private readonly IMqttClient _mqttClient;
        private readonly MqttConfig _mqttConfig;
        private readonly ILogger<MqttService> _logger;
        private readonly IServiceProvider _serviceProvider;

        private int _reconnectDelay = 5;
        private const int MAX_RECONNECT_DELAY = 30;

        public MqttService(IOptions<MqttConfig> mqttConfig, ILogger<MqttService> logger, IServiceProvider serviceProvider)
        {
            _mqttConfig = mqttConfig.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initializing MQTT Service...");

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_mqttConfig.BrokerHost, _mqttConfig.BrokerPort)
                .WithClientId($"SalesDashboard_{Guid.NewGuid()}")
                .WithCleanSession()
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .WithTimeout(TimeSpan.FromSeconds(10))
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += HandleMessageAsync;
            _mqttClient.DisconnectedAsync += HandleDisconnectAsync;

            _mqttClient.ConnectedAsync += HandleConnectedAsync;

            await ConnectAsync(options);

            if (_mqttClient.IsConnected)
            {
                _logger.LogInformation("Sucessfully connected to MQTT broker");
                await SubscribeToTopicsAsync();
            }
        }


        private async Task HandleConnectedAsync(MqttClientConnectedEventArgs  e)
        {
            _logger.LogInformation("Successfully connected to MQTT broker");
            _reconnectDelay = 5;
        }


        private async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var payload = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var topic = e.ApplicationMessage.Topic;

                switch (topic)
                {
                    case "ecommerce/new-order":
                        await HandleNewOrderAsync(payload);
                        break;

                    case "ecommerce/revenue-update":
                        await HandleRevenueUpdateAsync(payload);
                        break;

                    case "ecommerce/sales-update":
                        await HandleSalesUpdateAsync(payload);
                        break;

                    case "ecommerce/stock-update":
                        await HandleStockUpdateAsync(payload);
                        break;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling MQTT");
                throw new ApplicationException("Error while handling MQTT messages");
            }
        }


        private async Task HandleNewOrderAsync(string payload)
        {
            var order = JsonSerializer.Deserialize<Order>(payload);

            //using var scope = _serviceProvider.CreateScope();
            //var dbContext = scope.ServiceProvider.GetRequiredService<SalesDashBoardDbContext>();

            await PublishUpdateAsync("ecommerce/new-order", order.OrderId);
            _logger.LogInformation($"New order notification sent for Order ID : {order.OrderId}");            
        }


        private async Task HandleRevenueUpdateAsync(string payload)
        {
            var revenueData = JsonSerializer.Deserialize<Revenue>(payload);
            await PublishUpdateAsync("ecommerce/revenue-update", revenueData.Date);
            _logger.LogInformation($"Revenue Update notification sent for date : {revenueData.Date}");
        }


        private async Task HandleSalesUpdateAsync(string payload)
        {
            var salesData = JsonSerializer.Deserialize<SalesPerformance>(payload);
            await PublishUpdateAsync("ecommerce/sales-update", salesData.Date);
            _logger.LogInformation($"Sales Update notification sent for date : {salesData.Date}");
        }


        private async Task HandleStockUpdateAsync(string payload)
        {
            var stockData = JsonSerializer.Deserialize<Inventory>(payload);
            await PublishUpdateAsync("ecommerce/stock-update", stockData.ProductId);
            _logger.LogInformation($"Stock update notification sent for product ID : {stockData.ProductId}");
        }


        private async Task HandleDisconnectAsync(MqttClientDisconnectedEventArgs e)
        {
            _logger.LogWarning("MQTT Client DisConnected. Attempting reconnection...");
            await Task.Delay(TimeSpan.FromSeconds(_reconnectDelay));

            try
            {
                await _mqttClient.ReconnectAsync();
                _logger.LogInformation("Reconnection Successful");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reconnect MQTT client");
                _reconnectDelay = Math.Min(_reconnectDelay * 2, MAX_RECONNECT_DELAY);
            }
        }


        private async Task SubscribeToTopicsAsync()
        {
            var topics = new[]
            {
                "ecommerce/new-order",
                "ecommerce/revenue-update",
                "ecommerce/sales-update",
                "ecommerce/stock-update"
            };

            var subscribeOptions = new MqttClientSubscribeOptionsBuilder();

            foreach (var topic in topics)
                subscribeOptions.WithTopicFilter(topic);

            await _mqttClient.SubscribeAsync(subscribeOptions.Build());
        }


        public async Task PublishUpdateAsync<T>(string topic, T data)
        {
            var payload = JsonSerializer.Serialize(data);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_mqttClient.IsConnected)
                await _mqttClient.DisconnectAsync();
        }

        private async Task ConnectAsync(MqttClientOptions options)
        {
            try
            {
                _logger.LogInformation("initializing connection to MQTT broker...");
                using var timeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                await _mqttClient.ConnectAsync(options);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to MQTT broker : {message}", ex.Message);
                throw new ApplicationException("Failed to Connect to MQTT Broker");
            }
        }
    }


    // registration in program.cs
    public static class MqttServiceExtensions
    {
        public static IServiceCollection AddMqttService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MqttConfig>(configuration.GetSection("Mqtt"));
            services.AddSingleton<MqttService>();
            services.AddHostedService(sp => sp.GetRequiredService<MqttService>());

            return services;
        }
    }
}
