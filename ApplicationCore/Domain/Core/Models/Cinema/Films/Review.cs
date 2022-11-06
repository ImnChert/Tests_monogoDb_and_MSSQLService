using ApplicationCore.Domain.Core.Models.Roles;

namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Review : EntityBase
	{
		public RegisteredUser RegisteredUser { get; set; }
		public string Description { get; set; }
	}
}
