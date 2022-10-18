using ApplicationCore.Interfaces;

namespace ApplicationCore.Models.Roles.Staff
{
	internal class Employee : IUser
	{
		// TODO: Работник
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public List<IPosition> Positions { get; set; }
	}
}
