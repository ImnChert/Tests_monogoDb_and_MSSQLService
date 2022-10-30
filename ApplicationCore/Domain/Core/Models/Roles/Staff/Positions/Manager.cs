using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff.Positions
{
	public class Manager : IPosition
	{
		// TODO: управляющий
		public override string? ToString()
			=> "Manager";
	}
}
