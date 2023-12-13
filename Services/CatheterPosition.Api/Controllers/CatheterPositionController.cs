using CatheterPosition.Api.Services;
using Common.Config.Interface;
using ConfigurationManager.Implement;
using MessageBus.Infra.Implement;
using MessageBus.Infra.Interface;
using MessageBus.Infra.Reply.PositionService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Timers;
using CatheterPosition.Models;
using MessageBus.Infra;
using MessageBus.Infra.Requests.PositionService;
using Common.Hub;
using Microsoft.AspNetCore.SignalR;

namespace CatheterPosition.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CatheterPositionController : ControllerBase
	{
		private static readonly System.Timers.Timer _moveCatheterTimer = new(20);
		// private readonly IPositionService _positionService;
		private ILogger<CatheterPositionController> _logger;
		private readonly IRabbitChannelConfig _positionChannelConfig;
		private readonly IMessageConsumer _messageConsumer;
		private readonly IMessageProducer _messageProducer;
		private readonly IHubContext<CatheterPositionHub> _hub;

		public CatheterPositionController(IOptions<RabbitChannelConfig> channelsConfig, IHubContext<CatheterPositionHub> hub,
			IMessageProducer messageProducer, IMessageConsumer messageConsumer, ILogger<CatheterPositionController> logger)
		{
			_positionChannelConfig = channelsConfig.Value;
			_hub = hub;
			_messageConsumer = messageConsumer;
			_messageProducer = messageProducer;
			// _positionService = positionService;
			_logger = logger;

			_moveCatheterTimer.Elapsed += onCatheterTimerElapsed;
			_ = subscribeForReply();
		}


		[HttpGet]
		[Route("/GetCurrentPosition")]
		public Task<CatheterPositionData> Get()
		{ 
			var req = new CommandMessage(Guid.NewGuid().ToString(), CommandType.GET_CATHETER_POSITION);

			var moveCatheterChannelFound = _positionChannelConfig.TryGetChannelData("MoveCatheter", out var moveCatheterChannel);
			if (moveCatheterChannelFound)
			{
				_ = _messageProducer.SendMessageToTopic(req, moveCatheterChannel);
			}

			var p = new CatheterPositionData();

			return Task.FromResult(p);

		}

		[HttpHead]
		[Route("/StartSimulation", Name = "StartSimulation")]
		public Task StartSim()
		{
			// _messageProducer.SendLogRequest("[PositionController::StartSimulation] position controller started", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Debug("[PositionController::StartSimulation]");
			if (!_moveCatheterTimer.Enabled)
			{
				_moveCatheterTimer.Start();
			}
			return Task.CompletedTask;
		}


		[HttpHead]
		[Route("/StopSimulation", Name = "StopSimulation")]
		public Task StopSim()
		{
			// _messageProducer.SendLogRequest("[PositionController::StopSim] position controller started", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Debug("[PositionController::StopSim]");
			if (_moveCatheterTimer.Enabled)
			{
				_moveCatheterTimer.Stop();
				var moveCatheterChannelFound = _positionChannelConfig.TryGetChannelData("MoveCatheter", out var moveCatheterChannel);
				if (moveCatheterChannelFound)
				{
					var req = new CommandMessage(Guid.NewGuid().ToString(), CommandType.STOP_MOVE_CATHETER);
					_ = _messageProducer.SendMessageToTopic(req, moveCatheterChannel);
				}
			}

			return Task.CompletedTask;
		}


		[HttpHead]
		[Route("/ResetPosition", Name = "ResetPosition")]
		public Task ResetPosition()
		{
			// _messageProducer.SendLogRequest("[PositionController::ResetPosition] position controller started", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Debug("[PositionController::ResetPosition]");

			var resetPosChannelFound = _positionChannelConfig.TryGetChannelData("ResetPosition", out var resetPosChannel);
			var getPositionChannelFound = _positionChannelConfig.TryGetChannelData("GetCurrentPosition", out var getPositionChannel);

			var resetPositionRequest = new CommandMessage(Guid.NewGuid().ToString(), CommandType.RESET_CATHETER_POSITION);
			if (resetPosChannelFound)
			{
				_ = _messageProducer.SendMessageToTopic(resetPositionRequest, resetPosChannel);
			}

			var getPositionRequest = new CommandMessage(Guid.NewGuid().ToString(), CommandType.GET_CATHETER_POSITION);
			if (getPositionChannelFound)
			{
				_ = _messageProducer.SendMessageToTopic(getPositionRequest, getPositionChannel);
			}

			return Task.CompletedTask;
		}





		private Task subscribeForReply()
		{
			// _logger.LogDebug("[PositionController::subscribeForReply]");
			// _messageProducer.SendLogRequest("[PositionController::subscribeForReply]", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Information("[PositionController::subscribeForReply]");

			var catheterPositionReplyChannelFound =
				_positionChannelConfig.TryGetChannelData("CatheterPositionReply", out var catheterPositionReplyChannel);

			if (catheterPositionReplyChannelFound)
			{
				_ = _messageConsumer.SubscribeForTopic<CatheterPositionReply>(catheterPositionReplyChannel, onCatheterPositionReply);
			}
			return Task.CompletedTask;
		}



		private int rightCount = 0, downCount = 0, leftCount = 0, upCount = 0;
		private void onCatheterTimerElapsed(object sender, ElapsedEventArgs e)
		{
			//  TODO: get channels once!
			var moveCatheterChannelFound = _positionChannelConfig.TryGetChannelData("MoveCatheter", out var moveCatheterChannel);
			var getPositionChannelFound = _positionChannelConfig.TryGetChannelData("GetCurrentPosition", out var getPositionChannel);
			MoveCatheterCommand moveRequest = null;
			if (rightCount < 500)
			{
				moveRequest = new MoveCatheterCommand(Guid.NewGuid().ToString(), MoveDirection.RIGHT, .1);
			}
			else if (downCount < 250)
			{
				moveRequest = new MoveCatheterCommand(Guid.NewGuid().ToString(), MoveDirection.DOWN, .1);
			}
			else if (leftCount < 500)
			{
				moveRequest = new MoveCatheterCommand(Guid.NewGuid().ToString(), MoveDirection.LEFT, .1);
			}
			else if (upCount < 250)
			{
				moveRequest = new MoveCatheterCommand(Guid.NewGuid().ToString(), MoveDirection.UP, .1);
			}
			else
			{
				rightCount = 0;
				downCount = 0;
				leftCount = 0;
				upCount = 0;
			}

			if (moveRequest != null)
			{
				var positionRequest = new CommandMessage(Guid.NewGuid().ToString(), CommandType.GET_CATHETER_POSITION);

				if (moveCatheterChannelFound)
				{
					_ = _messageProducer.SendMessageToTopic(moveRequest, moveCatheterChannel);
				}
				if (getPositionChannelFound)
				{
					_ = _messageProducer.SendMessageToTopic(positionRequest, getPositionChannel);
				}
			}
		}

		private async Task onCatheterPositionReply(CatheterPositionReply position)
		{
			try
			{
				var cancellationToken = new CancellationToken();

				// TODO: check which clients should be receiving this message 
				// await _hub.Clients.All.SendAsync("PositionReceived", position, cancellationToken);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				// _logger.LogError(e, $"[PositionController::onCatheterPositionReply] Error -> {e.Message}");
				// _ = _messageProducer.SendLogRequest($"[PositionController::onCatheterPositionReply] Error -> {e.Message}", LogLevel.Error, _logRequestChannel, _assemblyName, e);
				// Log.Error(e, $"[PositionController::onCatheterPositionReply] Error -> {e.Message}");
				throw; // TODO: ??
			}
		}
	}
}
