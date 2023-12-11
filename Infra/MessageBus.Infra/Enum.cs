
namespace MessageBus.Infra
{
	public enum CommandType
	{
		MOVE_CATHETER = 0,
		GET_CATHETER_POSITION,
		RESET_CATHETER_POSITION,
		STOP_MOVE_CATHETER,

		GET_ALL_PLANS,
		GET_SINGLE_PLAN,
		ADD_NEW_PLAN
	}

	public enum LogLevel
	{
		/// <summary>
		/// Anything and everything you might want to know about
		/// a running block of code.
		/// </summary>
		Verbose,

		/// <summary>
		/// Internal system events that aren't necessarily
		/// observable from the outside.
		/// </summary>
		Debug,

		/// <summary>
		/// The lifeblood of operational intelligence - things
		/// happen.
		/// </summary>
		Information,

		/// <summary>
		/// Service is degraded or endangered.
		/// </summary>
		Warning,

		/// <summary>
		/// Functionality is unavailable, invariants are broken
		/// or data is lost.
		/// </summary>
		Error,

		/// <summary>
		/// If you have a pager, it goes off when one of these
		/// occurs.
		/// </summary>
		Fatal
	}
	
}
