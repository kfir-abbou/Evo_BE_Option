using Serilog;
using System.Reflection;
using System.Diagnostics;
using Common.Hub;

namespace FE.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var config = initConfig();

			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(config));

			var httpClient = new HttpClient();


			builder.Services.AddSingleton(httpClient);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddSignalR();
			builder.Services.AddCors();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});

			var app = builder.Build();

			app.UseSerilogRequestLogging();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				// app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<CatheterPositionHub>("/PositionHub");
			});

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}



		private static IConfigurationRoot initConfig()
		{
			var codeBase = Assembly.GetEntryAssembly().CodeBase;
			var uri = new UriBuilder(codeBase).Uri;
			var buildDirectory = Path.GetDirectoryName(uri.LocalPath);
			var loggingConfigFile = Path.Combine(buildDirectory, "Config", "Logging.Service.Config.json");

			if (!File.Exists(loggingConfigFile))
			{
				throw new FileNotFoundException(loggingConfigFile);
			}
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(loggingConfigFile)
				.Build();

			return config;
		}
	}
}