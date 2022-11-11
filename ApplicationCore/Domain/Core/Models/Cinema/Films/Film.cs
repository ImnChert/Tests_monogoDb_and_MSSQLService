namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Film : EntityBase
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

		public override bool Equals(object? obj)
		{
			return obj is Film film &&
				   Name == film.Name &&
				   EqualityComparer<List<Person>>.Default.Equals(FilmCrew, film.FilmCrew) &&
				   Year == film.Year &&
				   LicensExpirationDate == film.LicensExpirationDate &&
				   EqualityComparer<Distributor>.Default.Equals(Distributor, film.Distributor);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, FilmCrew, Year, LicensExpirationDate, Distributor);
		}
	}
}
