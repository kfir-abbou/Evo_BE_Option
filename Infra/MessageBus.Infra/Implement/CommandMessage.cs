
using MessageBus.Infra.Interface;

namespace MessageBus.Infra.Implement
{
	public class CommandMessage : ICommandMsg
	{
		public string Id { get; set; }
		public CommandType CommandType { get; set; }

		public CommandMessage(string id, CommandType type)
		{
			Id = id;
			CommandType = type;
		}
	}


}
