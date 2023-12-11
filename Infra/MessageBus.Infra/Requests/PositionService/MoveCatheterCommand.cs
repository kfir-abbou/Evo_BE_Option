using CatheterPosition.Models;
using MessageBus.Infra.Interface;

namespace MessageBus.Infra.Requests.PositionService
{
	public class MoveCatheterCommand : IMessage
	{
		public string Id { get; set; }
		public MoveDirection Direction { get; set; }
		public double Steps { get; set; }

		public MoveCatheterCommand(string id, MoveDirection direction, double steps)
		{
			Id = id;
			Direction = direction;
			Steps = steps;
		}
	}
}
