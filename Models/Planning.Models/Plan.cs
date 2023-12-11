using System.Runtime.Serialization;

namespace Planning.Models
{

	public class Plan
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public DateTime Date { get; set; }

		public Plan()
		{
			
		}

		public Plan(string id, string name, DateTime date)
		{
			Id = id;
			Name = name;
			Date = date;
		}
		public override string ToString()
		{
			return $"Id: {Id}, Name: {Name}, Date: {Date.ToShortDateString()}";
		}
	}
}
