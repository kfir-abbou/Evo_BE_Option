using System.Reflection;
using MessageBus.Infra.Interface;

namespace MessageBus.Infra.Requests.LoggingService
{
	public class LogRequest : IRequest
	{
		public string Id { get; set; }
		public string Message { get; set; }
		public LogLevel LogLevel { get; set; }
		public Exception Exception { get; set; }
		public string Assembly { get; set; }

		public LogRequest()
		{
			
		}

		public LogRequest(string id, string message, LogLevel level, string assembly, Exception exception = null)
		{
			Id = id;
			Message = message;
			LogLevel = level;
			Assembly = assembly;
			Exception = exception;
		}

	}
}
