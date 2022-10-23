namespace ApplicationCore.Models.Cinema.Films
{
	internal class Film  
	{
		public int Id { get; set; }
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

		public Film(int id, string name, TimeSpan duration, List<Person> filmCrew, 
			List<Review> reviews, string discription, int year, double raiting, 
			DateTime licensExpirationDate, Distributor distributor, decimal basePrice)
		{
			Id = id;
			Name = name;
			Duration = duration;
			FilmCrew = filmCrew;
			Reviews = reviews;
			Discription = discription;
			Year = year;
			Raiting = raiting;
			LicensExpirationDate = licensExpirationDate;
			Distributor = distributor;
			BasePrice = basePrice;
		}

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
