using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Person : EntityBase, IFullName
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Post { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is Person person &&
				   FirstName == person.FirstName &&
				   MiddleName == person.MiddleName &&
				   LastName == person.LastName;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstName, MiddleName, LastName);
		}
	}
}
