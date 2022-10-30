using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff.Positions
{
	public class Admin : IPosition
	{
		// TODO: Адми
		public override string? ToString()
			=> "Admin";
	}
}
