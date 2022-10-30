using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Person : EntityBase, IFullName
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Post { get; set; }

		public Person(string firstName, string middleName, string lastName, string post)
		{
			FirstName = firstName;
			MiddleName = middleName;
			LastName = lastName;
			Post = post;
		}
	}
}
