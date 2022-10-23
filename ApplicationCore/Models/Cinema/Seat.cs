namespace ApplicationCore.Models.Cinema
{
	internal class Seat
	{
		public int Id { get; set; }
		public int NumberRow { get; set; }
		public int NumberColumn { get; set; }
		public Category Category { get; }
	}
}
