using CatheterPosition.Models;
using MessageBus.Infra.Interface;
using MessageBus.Infra.Requests.PositionService;

namespace CatheterPosition.Api.Services
{

	public class PositionService : IPositionService
	{
		private static readonly IPosition _currentPosition = new Models.CatheterPositionData();
		private readonly ILogger<PositionService> _logger;
		private readonly IMessageProducer _messageProducer;
		private readonly IMessageConsumer _messageConsumer;
		private MoveCatheterCommand _moveCatheterCommand;
		private readonly IConfiguration _configuration;


		public PositionService(ILogger<PositionService> logger, IMessageProducer messageProducer, IMessageConsumer messageConsumer, IConfiguration config)
		{
			_messageProducer = messageProducer;
			_messageConsumer = messageConsumer;
			_configuration = config;
			_logger = logger;
		}

		public Task MoveCatheter(MoveDirection direction, double steps = 1)
		{
			_moveCatheterCommand = new MoveCatheterCommand(Guid.NewGuid().ToString(), direction, steps);
		 
			// _messageProducer.SendMessageToTopic(_moveCatheterCommand, )
			return Task.CompletedTask;
		}

		public IPosition GetCurrentPosition()
		{
			return _currentPosition;
		}

		public Task ResetPosition()
		{
			// _logger.Debug("[PositionService::ResetPosition]");
			_currentPosition.Reset();
			return Task.CompletedTask;
		}
	}

	public interface IPositionService
	{
		Task MoveCatheter(MoveDirection direction, double step = 1);
		IPosition GetCurrentPosition();
		Task ResetPosition();
	}
}
