using System.Text;
using Common.Config.Interface;
using MessageBus.Infra.Interface;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace MessageBus.Infra.Implement
{
	public class MessageConsumer : IMessageConsumer
	{
		private readonly IConnectionFactory _factory;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		// private readonly Dictionary<string, IModel> _channels;
		// private readonly Dictionary<string, string> _exchangesDictionary;


		public MessageConsumer(IConnectionFactory factory)
		{
			_factory = factory;
			// _channels = new Dictionary<string, IModel>();
			// _exchangesDictionary = new Dictionary<string, string>();
			_connection = _factory.CreateConnection();
			_channel = _connection.CreateModel();
		}


		public Task SubscribeForTopic<TMessage>(IChannelData channelData, Func<TMessage, Task> callbackAction)
		{
			try
			{
				_channel.ExchangeDeclare(channelData.Exchange, ExchangeType.Topic);
				var queueName = _channel.QueueDeclare().QueueName;
				_channel.QueueBind(queue: queueName, exchange: channelData.Exchange, routingKey: channelData.RoutingKey);

				var consumer = new EventingBasicConsumer(_channel);

				consumer.Received += (sender, args) =>
				{
					var msg = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(args.Body.ToArray()));
					if (msg != null)
					{
						_ = callbackAction(msg);
					}
				};

				_channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			return Task.CompletedTask;
		}

	}
}
