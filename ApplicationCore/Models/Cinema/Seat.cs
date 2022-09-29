namespace ApplicationCore.Models.Cinema
{
	internal class Seat
	{
		public int Id { get; set; }
		public Categories Category { get; }
		public bool Reservation { get; set; } = false;
	}
}
