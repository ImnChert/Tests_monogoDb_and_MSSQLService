namespace ApplicationCore.Models.Cinema.Films
{
	internal class Film  
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TimeSpan Duration { get; set; }
		public List<Director> Directors { get; set; } = new List<Director>();
		public List<Actor> Actors { get; set; }	= new List<Actor>();
		public List<Review> Reviews { get; set; } = new List<Review>();
		public string Discription { get; set; }
		public int Year { get; set; }
		public double Raiting { get; set; }
		public DateTime LicensExpirationDate { get; set; }
		public Distributor Distributor { get; set; }


		public Film(int id, string name, TimeSpan duration, List<Director> directors, 
			List<Actor> actors, List<Review> reviews, string discription, int year, 
			double raiting, DateTime licensExpirationDate, Distributor distributor)
		{
			Id = id;
			Name = name;
			Duration = duration;
			Directors = directors;
			Actors = actors;
			Reviews = reviews;
			Discription = discription;
			Year = year;
			Raiting = raiting;
			LicensExpirationDate = licensExpirationDate;
			Distributor = distributor;
		}

		public override bool Equals(object? obj)
		{
			return obj is Film film &&
				   Name == film.Name &&
				   Duration.Equals(film.Duration) &&
				   EqualityComparer<List<Director>>.Default.Equals(Directors, film.Directors) &&
				   EqualityComparer<List<Actor>>.Default.Equals(Actors, film.Actors) &&
				   Discription == film.Discription &&
				   Year == film.Year;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Duration, Directors, Actors, Discription, Year);
		}
	}
}
