using Common.Config.Interface;

namespace MessageBus.Infra.Interface
{
	public interface IMessageConsumer
	{
		Task SubscribeForTopic<TMessage>(IChannelData channelData, Func<TMessage, Task> callbackAction);
	}
}
