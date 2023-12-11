
namespace MessageBus.Infra.Interface
{
	public interface ICommandMsg : IMessage
	{
		public CommandType CommandType { get; set; }

	}
}
