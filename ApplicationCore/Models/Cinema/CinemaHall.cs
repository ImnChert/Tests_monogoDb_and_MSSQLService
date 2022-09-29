using ApplicationCore.Interfaces.Observer;

namespace ApplicationCore.Models.Cinema
{
	internal interface CinemaHall : ISubject
	{
		public int Id { get; set; }

		public Schedule Schedule { get; set; }
		public List<Seat> Seats { get; set; }
		public Ticket Ticket { get; set; } // Что-то не то
										   // TODO: доделать
	}
}
