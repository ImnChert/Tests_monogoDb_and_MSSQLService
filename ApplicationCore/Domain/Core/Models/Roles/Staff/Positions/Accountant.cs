using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Domain.Core.Models.Roles.Staff.Positions
{
	public class Accountant : IPosition
	{
		// TODO: Бухгалтер
		public override string? ToString()
			=> "Accountant";
	}
}
