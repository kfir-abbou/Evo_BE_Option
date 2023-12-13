using Microsoft.AspNetCore.Mvc;
using Planning.Api.Services;
using Planning.Models;

namespace Planning.Service.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class PlanningController : ControllerBase
	{
		private readonly IPlanningService _planningService;

		public PlanningController(ILogger<PlanningController> logger, IPlanningService planningService)
		{
			_planningService = planningService;
		}

		[HttpGet]
		[Route("/Plans")]
		public async Task<IEnumerable<Plan>> Get() // TODO: public IActionResult get
		{
			// var getPlansChannelFound = _channelsConfig.TryGetChannelData("GetAllPlansRequest", out var channel);
			// var allPlansRequest = new CommandMessage(Guid.NewGuid().ToString(), CommandType.GET_ALL_PLANS);
			// if (getPlansChannelFound)
			// {
			// 	_ = _messageProducer.SendMessageToTopic(allPlansRequest, channel);
			// 	// _ = _messageProducer.SendLogRequest($"[PlanningController::Get] send get all plans request, Channel data:{channel}", LogLevel.Information, _logRequestChannel, _assemblyName);
			// 	Log.Information($"[PlanningController::Get] send get all plans request, Channel data:{channel}");
			// }
			var plans = await _planningService.GetAllAsync();
			return plans;
		}


		[HttpGet]
		[Route("/Plans/{id}")]
		public async Task<Plan> Get(string id)
		{
			// // _logger.LogInformation($"[PlanningController::GetPlanById] Id: {id}");
			// // _messageProducer.SendLogRequest($"[PlanningController::GetPlanById] Id: {id}", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Information($"[PlanningController::GetPlanById] Id: {id}");
			//
			// var singlePlanRequest = new GetSinglePlanRequest(id);
			//
			// var planChannelFound = _channelsConfig.TryGetChannelData("GetSinglePlan", out var getPlanChannel);
			// if (planChannelFound)
			// {
			// 	_ = _messageProducer.SendMessageToTopic(singlePlanRequest, getPlanChannel);
			// }

			var plan = await _planningService.GetAsync(id);

			if (plan is null)
			{
				// return base.HttpContext.Response.SendOkAsync(response, base.Definition.SerializerContext, cancellation);
				throw new NullReferenceException($"Plan with id {id} not found");
			}

			return plan;
		}

		[HttpPost]
		[Route("/Plans")]
		public Task Add(Plan plan)
		{
			// // _logger.LogInformation($"[PlanningController::AddNewPlan]");
			// // _messageProducer.SendLogRequest("[PlanningController::AddNewPlan]", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Information("[PlanningController::AddNewPlan]");
			//
			// var addPlanRequest = new AddNewPlanRequest(plan) { Id = Guid.NewGuid().ToString() };
			//
			// var addPlanChannelFound = _channelsConfig.TryGetChannelData("AddNewPlan", out var addPlanChannel);
			// if (addPlanChannelFound)
			// {
			// 	_ = _messageProducer.SendMessageToTopic(addPlanRequest, addPlanChannel);
			// 	// _logger.LogInformation($"[PlanningController::AddNewPlan] send add new plan request, Plan data: {plan}");
			// 	// _messageProducer.SendLogRequest($"[PlanningController::AddNewPlan] send add new plan request, Plan data: {plan}", LogLevel.Information, _logRequestChannel, _assemblyName);
			// 	Log.Information($"[PlanningController::AddNewPlan] send add new plan request, Plan data: {plan}");
			// }


			_planningService.CreateAsync(plan);
			return Task.CompletedTask;
		}

		[HttpDelete]
		[Route("/Plans/{id}")]
		public async Task Delete(string id)
		{
			// // _logger.LogInformation($"[PlanningController::AddNewPlan]");
			// // _messageProducer.SendLogRequest("[PlanningController::AddNewPlan]", LogLevel.Information, _logRequestChannel, _assemblyName);
			// Log.Information("[PlanningController::AddNewPlan]");
			//
			// var addPlanRequest = new AddNewPlanRequest(plan) { Id = Guid.NewGuid().ToString() };
			//
			// var addPlanChannelFound = _channelsConfig.TryGetChannelData("AddNewPlan", out var addPlanChannel);
			// if (addPlanChannelFound)
			// {
			// 	_ = _messageProducer.SendMessageToTopic(addPlanRequest, addPlanChannel);
			// 	// _logger.LogInformation($"[PlanningController::AddNewPlan] send add new plan request, Plan data: {plan}");
			// 	// _messageProducer.SendLogRequest($"[PlanningController::AddNewPlan] send add new plan request, Plan data: {plan}", LogLevel.Information, _logRequestChannel, _assemblyName);
			// 	Log.Information($"[PlanningController::AddNewPlan] send add new plan request, Plan data: {plan}");
			// }


			await Task.Run(()=> _planningService.DeleteAsync(id)); // todo: change to real async 
			// return Task.CompletedTask;
		}

	}
}
