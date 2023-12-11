namespace CatheterPosition.Models
{
	public class CatheterPositionData : IPosition
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public double Yaw { get; set; }
		public double Pitch { get; set; }
		public double Roll { get; set; }
		public Task Reset()
		{
			X = 0;
			Y = 0;
			Z = 0;
			Yaw = 0;
			Pitch = 0;
			Roll = 0;
			return Task.CompletedTask;
		}

		public override string ToString()
		{
			return $"Location:{X}, {Y}, {Z}\tOrientation: {Yaw}, {Pitch}, {Roll}";
		}
	}

	public interface IPosition
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public double Yaw { get; set; }
		public double Pitch { get; set; }
		public double Roll { get; set; }

		Task Reset();

	}
}
