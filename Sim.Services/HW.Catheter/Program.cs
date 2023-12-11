using CatheterPosition.Models;
using Common.Config.Implement;
using Common.Config.Interface;
using ConfigurationManager.Implement;
using MessageBus.Infra;
using MessageBus.Infra.Implement;
using MessageBus.Infra.Interface;
using MessageBus.Infra.Reply.PositionService;
using MessageBus.Infra.Requests.PositionService;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace HW.Catheter
{
	public class Program
	{

		private static IMessageConsumer _msgConsumer = null!;
		private static IMessageProducer _msgProducer = null!;
		// private static IPositionService _positionService;
		private static ICommConfig _commConfig = null!;
		private static IRabbitChannelConfig _positionChannelsConfig = null!;
		private static IConfigurationRoot _config = null!;
		private static IPosition _currentPosition = new CatheterPositionData();
		static void Main(string[] args)
		{
			initConfig();

			initLogger(_config);

			initQueueComm();

			subscribeForMessages();

			Console.WriteLine("Starting position service api");
			Console.WriteLine("Waiting for messages...");
			Console.ReadLine();
		}

		private static void initConfig()
		{
			var commConfigFile = Path.Combine(Environment.CurrentDirectory, "Config", "comm.config.json");
			if (!File.Exists(commConfigFile))
			{
				throw new FileNotFoundException(commConfigFile);
			}
			var positionConfigFile =
				Path.Combine(Environment.CurrentDirectory, "Config", "CatheterPosition.Service.Config.json");
			if (!File.Exists(positionConfigFile))
			{
				throw new FileNotFoundException(positionConfigFile);
			}
			var loggingConfigFile = Path.Combine(Environment.CurrentDirectory, "Config", "Logging.Service.Config.json");
			if (!File.Exists(loggingConfigFile))
			{
				throw new FileNotFoundException(loggingConfigFile);
			}
			_config = new ConfigurationBuilder()
				.AddJsonFile(commConfigFile)
				.AddJsonFile(positionConfigFile)
				.AddJsonFile(loggingConfigFile)
				.AddEnvironmentVariables()
				.Build();

			_commConfig = _config.GetRequiredSection(nameof(CommConfig)).Get<CommConfig>()!;
			_positionChannelsConfig = _config.GetRequiredSection("CatheterPositionChannelsConfig").Get<RabbitChannelConfig>()!;
		}

		private static void initLogger(IConfigurationRoot? config)
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config!)
				.CreateLogger();
		}

		private static void initQueueComm()
		{
			// Log.Debug("[Program::initQueueComm]");
			var uri =
				$"{_commConfig.RabbitConnectionBaseUri}{_commConfig.UserName}:{_commConfig.Password}@{_commConfig.Ip}:{_commConfig.Port}";

			var factory = new ConnectionFactory
			{
				Uri = new Uri(uri),
				ClientProvidedName = "Plans Service",
				RequestedChannelMax = 3,
				RequestedHeartbeat = TimeSpan.FromSeconds(10)
				// DispatchConsumersAsync = true
			};
			_msgConsumer = new MessageConsumer(factory);
			_msgProducer = new MessageProducer(factory);
		}

		private static void subscribeForMessages()
		{
			Log.Debug("[Program::subscribeForMessages]");

			if (_positionChannelsConfig == null)
			{
				return;
				// throw?
			}

			var moveCatheterChannelFound = _positionChannelsConfig.TryGetChannelData("MoveCatheter", out var moveCatheterChannel);
			var getPositionChannelFound = _positionChannelsConfig.TryGetChannelData("GetCurrentPosition", out var getPositionChannel);
			var resetPositionChannelFound = _positionChannelsConfig.TryGetChannelData("ResetPosition", out var resetPositionChannel);

			if (moveCatheterChannelFound)
			{
				_msgConsumer.SubscribeForTopic<MoveCatheterCommand>(moveCatheterChannel, handleMoveCatheter);
				Log.Debug("[Program::subscribeForMessages] subscribed for 'MoveCatheter'");
			}

			if (getPositionChannelFound)
			{
				_msgConsumer.SubscribeForTopic<CommandMessage>(getPositionChannel, handleCommand);
				Log.Debug("[Program::subscribeForMessages] subscribed for 'GetCurrentPosition'");
			}

			if (resetPositionChannelFound)
			{
				_msgConsumer.SubscribeForTopic<CommandMessage>(resetPositionChannel, handleCommand);
				Log.Debug("[Program::subscribeForMessages] subscribed for 'ResetPosition'");
			}
		}

		private static Task handleMoveCatheter(MoveCatheterCommand message)
		{
			var direction = message.Direction;
			var steps = message.Steps;

			switch (direction)
			{
				case MoveDirection.UP:
					_currentPosition.Y += steps;
					break;
				case MoveDirection.DOWN:
					_currentPosition.Y -= steps;
					break;
				case MoveDirection.LEFT:
					_currentPosition.X -= steps;
					break;
				case MoveDirection.RIGHT:
					_currentPosition.X += steps;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}

			_currentPosition.Yaw += 0.00017;
			_currentPosition.Pitch += 0.000027;
			_currentPosition.Roll += 0.00037;
			return Task.CompletedTask;
		}

		private static Task handleCommand(CommandMessage cmd)
		{
			switch (cmd.CommandType)
			{
				case CommandType.MOVE_CATHETER:
				{
					// _ = handleMoveCatheter();
					break;
				}
				case CommandType.STOP_MOVE_CATHETER:
				{
					break;
				}
				case CommandType.GET_CATHETER_POSITION:
				{
					_ = handleGetPositionRequest();
					break;
				}
				case CommandType.RESET_CATHETER_POSITION:
				{
					_ = handleResetPosition();
					break;
				}
				case CommandType.GET_ALL_PLANS:
					break;
				case CommandType.GET_SINGLE_PLAN:
					break;
				case CommandType.ADD_NEW_PLAN:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return Task.CompletedTask;
		}

		private static Task handleGetPositionRequest()
		{
			// var pos = _positionService?.GetCurrentPosition();
			var pos = _currentPosition;
			var replyMsg = new CatheterPositionReply(pos);

			var catheterPosChannelFound = _positionChannelsConfig.TryGetChannelData("CatheterPositionReply", out var catheterPosChannel);
			if (catheterPosChannelFound)
			{
				_msgProducer.SendMessageToTopic(replyMsg, catheterPosChannel);
			}

			return Task.CompletedTask;
		}

		private static Task handleResetPosition()
		{
			Log.Debug("[Program::handleResetPosition]");

			// _ = _positionService?.ResetPosition();
			_currentPosition.Reset();
			return Task.CompletedTask;
		}

	}
}
