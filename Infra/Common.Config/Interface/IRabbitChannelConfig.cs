using Common.Config.Implement;

namespace Common.Config.Interface
{
	public interface IRabbitChannelConfig
	{
		List<ChannelData> ChannelsData { get; }

		bool TryGetChannelData(string id, out IChannelData channel);
	}
}
