namespace Common.Config.Interface
{
	public interface ICommConfig
	{
		string RabbitConnectionBaseUri { get; }
		string UserName { get; }
		string Password { get; }
		string Ip { get; }
		int Port { get; }
	}
}
