using ApplicationCore.Domain.Core.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff
{
	public class Employee : EntityBase, IFullName
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public List<Position> Positions { get; set; } = new List<Position>();
	}
}
