using Infrastructure.Models;

namespace ApplicationCore.Models.Cinema
{
	internal class Seat : EntityBase
	{
		public int NumberRow { get; set; }
		public int NumberColumn { get; set; }
		public Category Category { get; }
	}
}
