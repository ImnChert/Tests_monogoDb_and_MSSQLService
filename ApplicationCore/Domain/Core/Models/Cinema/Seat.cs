namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Seat : EntityBase
	{
		public int NumberRow { get; set; }
		public int NumberColumn { get; set; }
		public Category Category { get; }
	}
}
