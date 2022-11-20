namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Seat : EntityBase
	{
		public int NumberRow { get; set; }
		public int NumberColumn { get; set; }
		public Category Category { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is Seat seat &&
				   NumberRow == seat.NumberRow &&
				   NumberColumn == seat.NumberColumn;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(NumberRow, NumberColumn);
		}
	}
}
