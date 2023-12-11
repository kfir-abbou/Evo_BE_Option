using Planning.Models;

namespace Planning.Service.Repositories
{
	public class PlanningRepository : IPlanningRepository // TODO: Create IRepository<T>
	{
		private static IDictionary<string, Plan>? _planDb;
		public PlanningRepository()
		{
			_planDb = new Dictionary<string, Plan>
				{
					{
						"1", new Plan
						{
							Id = "1",
							Name = "Plan 1",
							Date = DateTime.Now
						}
					},
					{
						"2", new Plan
						{
							Id = "2",
							Name = "Plan 2",
							Date = DateTime.Now
						}
					},
					{
						"3", new Plan
						{
							Id = "3",
							Name = "Plan 3",
							Date = DateTime.Now
						}
					},
					{
						"4", new Plan
						{
							Id = "4",
							Name = "Plan 4",
							Date = DateTime.Now
						}
					},
					{
						"5", new Plan
						{
							Id = "5",
							Name = "Plan 5",
							Date = DateTime.Now
						}
					}
				};

		}

		public IEnumerable<Plan> GetAll()
		{
			return _planDb!.Values.ToList();
		}

		public Plan? GetPlan(string id)
		{
			if (_planDb!.ContainsKey(id))
			{
				return _planDb[id];
			}
			return null;
		}

		public async Task AddPlan(Plan plan)
		{
			await Task.Run(() => // TODO: call db async
			{
				if (_planDb != null)
				{
					_planDb.TryAdd(plan.Id, plan);
				}
			});
		}

		public Task RemovePlan(string id)
		{
			if (_planDb != null && _planDb.ContainsKey(id))
			{
				_planDb.Remove(id);
			}

			return Task.CompletedTask;
		}
	}
}



public interface IPlanningRepository
{
	IEnumerable<Plan> GetAll();
	Plan? GetPlan(string id);
	Task AddPlan(Plan plan);
	Task RemovePlan(string id);
}

