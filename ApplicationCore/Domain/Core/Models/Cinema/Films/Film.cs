namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Film  : EntityBase
	{
		public string Name { get; set; }
		public TimeSpan Duration { get; set; }
		public List<Person> FilmCrew { get; set; } = new List<Person>();
		public List<Review> Reviews { get; set; } = new List<Review>();
		public string Discription { get; set; }
		public int Year { get; set; }
		public double Raiting { get; set; }
		public DateTime LicensExpirationDate { get; set; }
		public Distributor Distributor { get; set; }
		public decimal BasePrice { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is Film film &&
				   Name == film.Name &&
				   Duration.Equals(film.Duration) &&
				   EqualityComparer<List<Person>>.Default.Equals(FilmCrew, film.FilmCrew) &&
				   EqualityComparer<List<Review>>.Default.Equals(Reviews, film.Reviews) &&
				   Discription == film.Discription &&
				   Year == film.Year &&
				   EqualityComparer<Distributor>.Default.Equals(Distributor, film.Distributor);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Duration, FilmCrew, Reviews, Discription, Year, Distributor);
		}
	}
}
