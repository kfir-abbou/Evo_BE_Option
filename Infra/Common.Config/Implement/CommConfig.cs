using Common.Config.Interface;

namespace Common.Config.Implement
{
	public class CommConfig : ICommConfig
	{
		public string RabbitConnectionBaseUri { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Ip { get; set; }
		public int Port { get; set; }


		public override string ToString()
		{
			return $"Uri: {RabbitConnectionBaseUri}{UserName}:{Password}@{Ip}:{Port}";
		}
	}
}
