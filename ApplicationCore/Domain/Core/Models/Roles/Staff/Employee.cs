using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff
{
	public class Employee : EntityBase
	{
		// TODO: Работник
		public string Username { get; set; }
		public string Password { get; set; }
		public List<IPosition>? Positions { get; set; }
	}
}
