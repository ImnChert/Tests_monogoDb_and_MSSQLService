using ApplicationCore.Interfaces;
using Infrastructure.Models;

namespace ApplicationCore.Models.Roles.Staff
{
	internal class Employee : EntityBase
	{
		// TODO: Работник
		public string Username { get; set; }
		public string Password { get; set; }
		public List<IPosition> Positions { get; set; }
	}
}
