using CatheterPosition.Api.Services;
using Common.Config.Implement;
using ConfigurationManager.Implement;
using MessageBus.Infra.Implement;
using MessageBus.Infra.Interface;
using RabbitMQ.Client;

namespace CatheterPosition.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var config = loadConfigurations(builder);
			var commConfig = config.GetRequiredSection(nameof(CommConfig)).Get<CommConfig>();
			IConnectionFactory factory = createRabbitConnectionFactory(commConfig);

			// Add services to the container.
			builder.Services.AddSingleton(factory);
			builder.Services.AddSingleton<IPositionService, PositionService>();
			builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
			builder.Services.AddSingleton<IMessageConsumer, MessageConsumer>();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowFrontEnd",
					b => b.WithOrigins("https://localhost:7121")
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});
			builder.Services.AddSignalR();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


			app.MapControllers();

			app.Run();
		}
		private static IConfigurationRoot loadConfigurations(WebApplicationBuilder builder)
		{
			var execDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var appPath = Path.GetDirectoryName(execDirectory);
			var commConfigFile = Path.Combine(appPath, "Config", "comm.config.json");
			var positionConfigFile = Path.Combine(appPath, "Config", "CatheterPosition.Service.Config.json");

			var config = builder.Configuration
				.AddJsonFile(commConfigFile).AddEnvironmentVariables()
				.AddJsonFile(positionConfigFile)
				.SetBasePath(Environment.CurrentDirectory)
				.Build();

			builder.Services.Configure<CommConfig>(config.GetSection(nameof(CommConfig)));
			builder.Services.Configure<RabbitChannelConfig>(config.GetSection("CatheterPositionChannelsConfig"));
			return config;
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
	}
}
