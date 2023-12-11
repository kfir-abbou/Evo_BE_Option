using CatheterPosition.Models;
using Microsoft.AspNetCore.SignalR;

namespace Common.Hub
{
	public class CatheterPositionHub : Microsoft.AspNetCore.SignalR.Hub
	{
		public async Task SendPositionString(string positionStr)
		{
			await Clients.All.SendAsync("PositionReceived", positionStr);
		}

		public async Task SendPosition(CatheterPositionData position)
		{
			await Clients.All.SendAsync("PositionReceived", position);
		}

		public override Task OnConnectedAsync()
		{
			Console.WriteLine("CatheterPosition Hub Connected");
			return base.OnConnectedAsync();
		}
	}
}
