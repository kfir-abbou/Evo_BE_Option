using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Planning.Models;

namespace MessageBusSystem.FE.Controllers
{
	public class PlanningController : Controller
	{
		private const string ServiceName = "DemoServiceName";
		// private readonly HttpClient _httpClient;
		// private readonly ActivitySource _activitySource;
		public PlanningController(HttpClient httpClient)
		{
			// _httpClient = httpClient;
			// _activitySource = new ActivitySource(ServiceName);
		}

		public Task<IActionResult> Index()
		{
			// Call API Gateway endpoint
			return Task.FromResult<IActionResult>(View());
		}

		public Task<IActionResult> AddPlan()
		{
			// Call API Gateway endpoint
			return Task.FromResult<IActionResult>(View());
		}


		// [HttpGet]
		// [Route("Plans")]
		// public async Task<IActionResult> Get()
		// {
		// 	// using var activity = _activitySource.StartActivity("Get Plans List");
		// 	var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7241/api/Plans");
		//
		// 	var response = await _httpClient.SendAsync(request);
		//
		// 	if (response.IsSuccessStatusCode)
		// 	{
		// 		var content = await response.Content.ReadAsStringAsync();
		// 		var plans = JsonConvert.DeserializeObject<List<Plan>>(content);
		// 		
		// 		return View("Index", plans);
		// 	}
		// 	else
		// 	{
		// 		// handle error
		// 	}
		// 	return BadRequest("Failed to fetch data from the API Gateway.");
		// }
		//
		// [HttpGet]
		// [Route("Plans/{id}")]
		// public async Task<IActionResult> Get(int id)
		// {
		// 	var url = $"https://localhost:7241/api/Plans/{id}";
		// 	var request = new HttpRequestMessage(HttpMethod.Get, url);
		//
		// 	var response = await _httpClient.SendAsync(request);
		//
		// 	if (response.IsSuccessStatusCode)
		// 	{
		// 		return RedirectToAction("Index");
		// 	}
		// 	else
		// 	{
		// 		// handle error
		// 	}
		// 	return BadRequest("Failed to fetch data from the API Gateway.");
		// }
		//
		//
		// [HttpPost]
		// public async Task<IActionResult> AddNewPlan([FromBody] Plan plan)
		// {
		// 	var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7241/api/Plans");
		// 	request.Content = new StringContent(JsonConvert.SerializeObject(plan), Encoding.UTF8, "application/json");
		// 	var response = await _httpClient.SendAsync(request);
		//
		// 	if (response.IsSuccessStatusCode)
		// 	{
		// 		return RedirectToAction("Index");
		// 	}
		// 	else
		// 	{
		// 		// handle error
		// 	}
		// 	return BadRequest("Failed to fetch data from the API Gateway.");
		// }
		//
		//
		// // POST api/<ValuesController>
		// [HttpPost]
		// public void Post([FromBody] string value)
		// {
		// }
		//
		// // PUT api/<ValuesController>/5
		// [HttpPut("{id}")]
		// public void Put(int id, [FromBody] string value)
		// {
		// }
		//
		// // DELETE api/<ValuesController>/5
		// [HttpDelete("{id}")]
		// public void Delete(int id)
		// {
		// }
	}
}
