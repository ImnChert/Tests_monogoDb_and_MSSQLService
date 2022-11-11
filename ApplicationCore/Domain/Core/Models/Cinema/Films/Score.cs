using ApplicationCore.Domain.Core.Models.Roles;

namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Score : EntityBase
	{
		public RegisteredUser RegisteredUser { get; set; }
		public int Raiting;
	}
}
