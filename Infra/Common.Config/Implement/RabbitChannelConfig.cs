using Common.Config.Implement;
using Common.Config.Interface;

namespace ConfigurationManager.Implement
{
	public class RabbitChannelConfig : IRabbitChannelConfig
	{
		public List<ChannelData> ChannelsData { get; init; }

		public bool TryGetChannelData(string id, out IChannelData channel)
		{
			channel = ChannelsData.SingleOrDefault(ch => ch.Id == id, new ChannelData(id));
			return channel != null;
		}

		public override string ToString()
		{
			return $"ChannelsData count: {ChannelsData.Count}";
		}
	}
}
