using MessageBus.Infra.Interface;
using RabbitMQ.Client;
using System.Text;
using Common.Config.Interface;
using Newtonsoft.Json;
using MessageBus.Infra.Requests.LoggingService;


namespace MessageBus.Infra.Implement
{
	public class MessageProducer : IMessageProducer
	{
		private readonly IConnectionFactory _factory;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly Dictionary<string, IModel> _channels;
		// readonly Dictionary<string, string> _exchanges = new();

		public MessageProducer(IConnectionFactory factory)
		{
			_factory = factory;
			_connection = _factory.CreateConnection();
			_channels = new Dictionary<string, IModel>();
			_channel = _connection.CreateModel();
		}


		// public Task PublishMessageAsync(IMessage message, string routing)
		// {
		// 	try
		// 	{
		// 		var jsonMsg = JsonConvert.SerializeObject(message);
		// 		var msgBody = Encoding.UTF8.GetBytes(jsonMsg);
		// 		var channel = _channels[routing];
		//
		// 		channel.ExchangeDeclare(exchange: "topics", type: ExchangeType.Topic);
		//
		// 		var properties = channel.CreateBasicProperties();
		// 		properties.DeliveryMode = 1;
		// 		channel.BasicPublish(exchange: "topics", routingKey: routing, basicProperties: properties,
		// 			mandatory: false, body: msgBody);
		// 		return Task.CompletedTask;
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		Console.WriteLine(e);
		// 	}
		// 	return Task.CompletedTask;
		// }
		//
		//
		public Task SendMessageToTopic(IMessage request, IChannelData channelData, IBasicProperties props = null)
		{
			try
			{
				var messageBody = getMessageBody(request);
				_channel.BasicPublish(exchange: channelData.Exchange, routingKey: channelData.RoutingKey, basicProperties: props, body: messageBody);
				Console.WriteLine($"Msg Sent: {channelData.Exchange}, {channelData.RoutingKey}, Id: {request.Id}");
				return Task.CompletedTask;
			}
			catch (Exception e)
			{
				// throw;
				Console.WriteLine("Error: " + e);
				return Task.CompletedTask;
			}
		}

		public Task SendLogRequest(string message, LogLevel level, IChannelData channel, string assembly, Exception ex = null)
		{
			var req = new LogRequest(Guid.NewGuid().ToString(), message, level, assembly, ex);
			_ = SendMessageToTopic(req, channel);
			return Task.CompletedTask;
		}

		private static byte[] getMessageBody<TMessage>(TMessage request) where TMessage : IMessage
		{
			var reqJson = JsonConvert.SerializeObject(request);
			var messageBody = Encoding.UTF8.GetBytes(reqJson);
			return messageBody;
		}

	}
}
