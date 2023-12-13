using CatheterPosition.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageBusSystem.FE.Controllers
{
	public class PositionController : Controller
	{
		private readonly HttpClient _httpClient;

		public PositionController(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public IActionResult Index()
		{
			// Call API Gateway endpoint
			return View(new CatheterPositionData());
		}

		public async Task<IActionResult> GetPosition()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7241/api/GetCurrentPosition");

			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				// var responseBytes = await response.Content.ReadAsByteArrayAsync();
				// var jsonString = Encoding.UTF8.GetString(responseBytes);
				// var position = JsonConvert.DeserializeObject<CatheterPosition>(jsonString);
				return RedirectToAction("Index");
			}
			else
			{
				// handle error
			}
			return BadRequest("Failed to fetch data from the API Gateway.");
		}

		public async Task<IActionResult> StartSim()
		{
			// Call API Gateway endpoint
			var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7241/api/StartSimulation");

			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				// var responseBytes = await response.Content.ReadAsByteArrayAsync();
				// var jsonString = Encoding.UTF8.GetString(responseBytes);
				// var position = JsonConvert.DeserializeObject<CatheterPosition>(jsonString);
				return RedirectToAction("Index");
			}
			else
			{
				// handle error
			}
			return BadRequest("Failed to fetch data from the API Gateway.");
		}

		public async Task<IActionResult> StopSim()
		{
			// Call API Gateway endpoint
			var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7241/api/StopSimulation");

			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				// var responseBytes = await response.Content.ReadAsByteArrayAsync();
				// var jsonString = Encoding.UTF8.GetString(responseBytes);
				// var position = JsonConvert.DeserializeObject<CatheterPosition>(jsonString);
				return RedirectToAction("Index");
			}
			else
			{
				// handle error
			}
			return BadRequest("Failed to fetch data from the API Gateway.");
		}

		public async Task<IActionResult> ResetPosition()
		{
			// Call API Gateway endpoint
			var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7241/api/ResetPosition");

			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else
			{
				// handle error
			}
			return BadRequest("Failed to fetch data from the API Gateway.");
		}
	}
}
