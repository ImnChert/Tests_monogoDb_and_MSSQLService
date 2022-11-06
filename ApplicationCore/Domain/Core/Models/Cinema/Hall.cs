namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Hall : EntityBase
	{ 
		public int Number { get; set; }
		public int Row { get; set; } = 10;
		public int Column { get; set; } = 10;
	}
}
