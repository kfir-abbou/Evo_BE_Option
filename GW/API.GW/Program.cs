
using Common.Config.Implement;
using Common.Hub;
using ConfigurationManager.Implement;
using MessageBus.Infra.Implement;
using MessageBus.Infra.Interface;
using Microsoft.AspNetCore.Connections;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using RabbitMQ.Client;
using Serilog;

namespace API.GW
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var config = loadConfigurations(builder);

			initLogger(config);
			Log.Information("[Program::Main] Starting api gateway app...");
			 
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowFrontEnd",
					b => b.WithOrigins("https://localhost:7117")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});
			builder.Services.AddSignalR();
			builder.Services.AddOcelot();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.UseCors("AllowFrontEnd");

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<CatheterPositionHub>("/PositionHub");
				endpoints.MapControllers();
			});

			app.MapControllers();

			app.UseOcelot().Wait();
			try
			{
				app.Run();
			}
			catch (Exception e)
			{
				// msgProducer.SendLogRequest("ERROR", LogLevel.Error, )
				Console.WriteLine(e);
				throw;
			}
		}

		private static ConnectionFactory createRabbitConnectionFactory(CommConfig commConfig)
		{
			var uri =
				$"{commConfig.RabbitConnectionBaseUri}{commConfig.UserName}:{commConfig.Password}@{commConfig.Ip}:{commConfig.Port}";
			var factory = new ConnectionFactory
			{
				Uri = new Uri(uri),
				ClientProvidedName = "Position API Service",
				RequestedChannelMax = 3,
				RequestedHeartbeat = TimeSpan.FromSeconds(10)
				// DispatchConsumersAsync = true
			};
			return factory;
		}

		private static IConfigurationRoot loadConfigurations(WebApplicationBuilder builder)
		{
			var execDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var appPath = Path.GetDirectoryName(execDirectory);
			// var commConfigFile = Path.Combine(appPath, "Config", "comm.config.json");
			// var planningConfigFile = Path.Combine(appPath, "Config", "Planning.Service.Config.json");
			// var positionConfigFile = Path.Combine(appPath, "Config", "CatheterPosition.Service.Config.json");
			var loggingConfigFile = Path.Combine(appPath, "Config", "Logging.Service.Config.json");

			var config = builder.Configuration
				// .AddJsonFile(commConfigFile)
				// .AddJsonFile(planningConfigFile)
				// .AddJsonFile(positionConfigFile)
				.AddJsonFile(loggingConfigFile)
				.AddEnvironmentVariables()
				.SetBasePath(Environment.CurrentDirectory)
				.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
				.Build();

			// builder.Services.Configure<CommConfig>(config.GetSection(nameof(CommConfig)));
			// builder.Services.Configure<RabbitChannelConfig>(config.GetSection("CatheterPositionChannelsConfig"));
			// builder.Services.Configure<RabbitChannelConfig>(config.GetSection("PlanningChannelsConfig"));
			builder.Services.Configure<RabbitChannelConfig>(config.GetSection("LoggingChannelsConfig"));
			return config;
		}

		private static void initLogger(IConfigurationRoot? config)
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config!)
				.CreateLogger();
		}
	}
}
