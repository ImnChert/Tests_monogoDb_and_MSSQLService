using ApplicationCore.Domain.Core.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff
{
	public class Employee : EntityBase, IFullName
	{
		// TODO: Работник
		[BsonElement("username")]
		public string Username { get; set; }

		[BsonElement("password")]
		public string Password { get; set; }

		[BsonElement("firstName")]
		public string FirstName { get; set; }

		[BsonElement("middleName")]
		public string MiddleName { get; set; }

		[BsonElement("lastName")]
		public string LastName { get; set; }

		[BsonElement("posts")]
		public List<Position> Positions { get; set; } = new List<Position>();
	}
}
