namespace ApplicationCore.Models.Cinema.Films
{
	internal class FilmDistribution : Film
	{
		public decimal BasePrice { get; set; }

		public FilmDistribution(int id, string name, TimeSpan duration, List<Director> directors,
			List<Actor> actors, List<Review> reviews, string discription, int year, double raiting,
			DateTime licensExpirationDate, Distributor distributor, decimal basePrice)
			: base(id, name, duration, directors, actors, reviews,
				  discription, year, raiting, licensExpirationDate, distributor)
		{
			BasePrice = basePrice;
		}
	}
}
