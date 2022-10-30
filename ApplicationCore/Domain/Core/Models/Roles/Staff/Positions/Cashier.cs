using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff.Positions
{
	public class Cashier : IPosition
	{
		public override string? ToString()
			=> "Cashier";
	}
}
