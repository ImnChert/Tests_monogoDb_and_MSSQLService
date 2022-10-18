using ApplicationCore.Models.Roles;

namespace ApplicationCore.Models.Cinema.Films
{
	internal class Review
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
