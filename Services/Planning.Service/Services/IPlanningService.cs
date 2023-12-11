using Planning.Models;

namespace Planning.Api.Services
{
	public class PlanningService : IPlanningService
	{
		private readonly IPlanningRepository _planningRepository;

		public PlanningService(IPlanningRepository planningRepository)
		{
			_planningRepository = planningRepository;
		}

		public Task CreateAsync(Plan plan)
		{
			var existingUser = _planningRepository.GetPlan(plan.Id);
			if (existingUser is not null)
			{
				throw new Exception("Plan already exist");
			}

			return _planningRepository.AddPlan(plan);
		}

		public Task<Plan?> GetAsync(string id)
		{
			return Task.FromResult(_planningRepository.GetPlan(id));
		}

		public Task<IEnumerable<Plan>> GetAllAsync()
		{
			var all = _planningRepository.GetAll();
			return Task.FromResult(all);
		}

		public Task<bool> UpdateAsync(Plan plan)
		{
			var planFromRepo = _planningRepository.GetPlan(plan.Id);

			if (planFromRepo != null)
			{
				_planningRepository.RemovePlan(plan.Id);
				_planningRepository.AddPlan(plan);
				return Task.FromResult(true);
			}
			return Task.FromResult(false);
		}

		public Task<bool> DeleteAsync(string id)
		{
			try
			{
				_planningRepository.RemovePlan(id);
				return Task.FromResult(true);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return Task.FromResult(false);
			}
		}
	}

	public interface IPlanningService
	{
		Task CreateAsync(Plan customer);

		Task<Plan?> GetAsync(string id);

		Task<IEnumerable<Plan>> GetAllAsync();

		Task<bool> UpdateAsync(Plan customer);

		Task<bool> DeleteAsync(string id);
	}
}
