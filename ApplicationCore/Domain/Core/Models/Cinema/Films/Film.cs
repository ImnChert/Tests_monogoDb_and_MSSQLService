namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Film  : EntityBase
	{
		public string Name { get; set; }
		public int Duration { get; set; }
		public List<Person> FilmCrew { get; set; } = new List<Person>();
		public List<Review> Reviews { get; set; } = new List<Review>();
		public string Description { get; set; }
		public int Year { get; set; }
		public List<Score> Scores { get; set; } = new List<Score>();
		public DateTime LicensExpirationDate { get; set; }
		public Distributor Distributor { get; set; }
		public decimal BasePrice { get; set; }
	}
}
