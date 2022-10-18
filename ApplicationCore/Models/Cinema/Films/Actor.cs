using ApplicationCore.Interfaces;

namespace ApplicationCore.Models.Cinema.Films
{
	internal class Actor : IFullName
	{
		public string FirstName { get; set; }
		public string MiddleName { get ; set; }
		public string LastName { get; set; }

		public Actor(string firstName, string middleName, string lastName)
		{
			FirstName = firstName;
			MiddleName = middleName;
			LastName = lastName;
		}
	}
}
