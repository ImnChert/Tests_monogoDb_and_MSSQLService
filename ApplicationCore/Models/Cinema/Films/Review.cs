using ApplicationCore.Models.Roles;
using Infrastructure.Models;

namespace ApplicationCore.Models.Cinema.Films
{
	internal class Review : EntityBase
	{
		RegisteredUser RegisteredUser { get; set; }
		public string Dicriprion { get; set; }

		public Review(RegisteredUser registeredUser, string dicriprion)
		{
			RegisteredUser = registeredUser;
			Dicriprion = dicriprion;
		}
	}
}
