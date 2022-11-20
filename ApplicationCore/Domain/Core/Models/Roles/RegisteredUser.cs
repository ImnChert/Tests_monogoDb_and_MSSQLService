using ApplicationCore.Domain.Core.Interfaces;
using ApplicationCore.Domain.Core.Interfaces.Observer;

namespace ApplicationCore.Domain.Core.Models.Roles
{
	public class RegisteredUser : Guest, IObserver, IAuthorization, IFullName
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirthday { get; set; }
		public string Phone { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is RegisteredUser user &&
				   Username == user.Username &&
				   Phone == user.Phone;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Username, Phone);
		}

		public void Update(ISubject subject, Exception exception)
		{

			throw exception;

		}


	}
}
