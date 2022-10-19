namespace ApplicationCore.Models.Cinema
{
	internal class Seat
	{
		public int Id { get; set; }
		public Category Category { get; }
		public bool Reservation { get; set; } = false;
	}
}
