using Common.Config.Interface;
using RabbitMQ.Client;

namespace MessageBus.Infra.Interface
{
	public interface IMessageProducer
	{
		Task SendMessageToTopic(IMessage request, IChannelData channelData, IBasicProperties props = null);
		Task SendLogRequest(string message, LogLevel level, IChannelData channel, string assembly, Exception ex = null);

	}
}