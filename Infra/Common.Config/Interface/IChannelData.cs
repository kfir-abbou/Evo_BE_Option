namespace Common.Config.Interface
{
	public interface IChannelData
	{
		string Id { get; }
		string Exchange { get; }
		string Queue { get; }
		string RoutingKey { get; }
	}
}
