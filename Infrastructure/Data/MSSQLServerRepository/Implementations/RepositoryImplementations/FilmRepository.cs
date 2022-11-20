using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class FilmRepository : MainMSSQLServerRepository<Film>
	{
		private IRepository<Distributor> _distributorRepository;
		private readonly IGetAllById<Review> _reviewGetAllById;
		private readonly IGetAllById<Person> _personGetAllById;
		private readonly IGetAllById<Score> _scoreGetAllById;

		public FilmRepository(string connectionString, IRepository<Distributor> distributorRepository,
			IGetAllById<Review> reviewGetAllById, IGetAllById<Person> personGetAllById, IGetAllById<Score> scoreGetAllById)
			: base(connectionString,
				  "Films",
				  $@"INSERT INTO Films (Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice) 
					VALUES(@Id,@Name,@Duration,@DescriptionFilm,@YearOfRelease,@LicensExpirationDate,@DistributorId,@BasePrice)",
				  $@"UPDATE Films SET Name=@Name Duration=@Duration, DescriptionFilm=@DescriptionFilm, 
					YearOfRelease=@YearOfRelease, LicensExpirationDate=@LicensExpirationDate,
					DistributorId=@DistributorId, BasePrice=@BasePrice
					WHERE Id=@Id",
				  $@"SELECT Films.Id, Name, Duration, DescriptionFilm, YearOfRelease, LicensExpirationDate, DistributorId, BasePrice, 
					Scores.Id, Scores.FilmId, Scores.Raiting, Scores.RegisteredUsersId,
					Reviews.Id, Reviews.FilmId, Reviews.DescriptionReview, Reviews.RegisteredUsersId, 
					People.Id, People.FilmsId, People.FirstName, People.LastName, People.MiddleName, People.Post
					FROM Films
					INNER JOIN Scores
					ON Scores.FilmId = Films.Id
					INNER JOIN Reviews
					ON Reviews.FilmId = Films.Id
					INNER JOIN People
					ON People.FilmsId = Films.Id",
				  $@"SELECT Films.Id, Name, Duration, DescriptionFilm, YearOfRelease, LicensExpirationDate, DistributorId, BasePrice, 
					Scores.Id, Scores.FilmId, Scores.Raiting, Scores.RegisteredUsersId,
					Reviews.Id, Reviews.FilmId, Reviews.DescriptionReview, Reviews.RegisteredUsersId, 
					People.Id, People.FilmsId, People.FirstName, People.LastName, People.MiddleName, People.Post
					FROM Films
					INNER JOIN Scores
					ON Scores.FilmId = Films.Id
					INNER JOIN Reviews
					ON Reviews.FilmId = Films.Id
					INNER JOIN People
					ON People.FilmsId = Films.Id
					WHERE Id=@id")
		{
			_distributorRepository = distributorRepository;
			_reviewGetAllById = reviewGetAllById;
			_personGetAllById = personGetAllById;
			_scoreGetAllById = scoreGetAllById;
		}

		public FilmRepository(string connectionString)
			: this(connectionString,
				 new DistributorRepository(connectionString),
				 new ReviewRepository(connectionString),
				 new PersonRepository(connectionString),
				 new ScoreRepository(connectionString))
		{
		}

		protected override Film GetReader(SqlDataReader sqlDataReader)
			=> new Film()
			{
				Id = (int)sqlDataReader["Id"],
				Name = sqlDataReader["Name"] as string ?? "Undefined",
				Duration = (int)sqlDataReader["Duration"],
				FilmCrew = _personGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result,
				Reviews = _reviewGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result,
				Description = sqlDataReader["Description"] as string ?? "Undefined",
				Year = (int)sqlDataReader["YearOfRelease"],
				Scores = _scoreGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result,
				LicensExpirationDate = DateTime.Parse((string)sqlDataReader["LicensExpirationDate"]),
				Distributor = _distributorRepository.GetById((int)sqlDataReader["DistributorId"]).Result,
				BasePrice = (decimal)sqlDataReader["BasePrice"]
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Film entity)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.NVarChar).Value = entity.Id;
			sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
			sqlCommand.Parameters.Add("@Duration", SqlDbType.NVarChar).Value = entity.Duration;
			sqlCommand.Parameters.Add("@DescriptionFilm", SqlDbType.NVarChar).Value = entity.Description;
			sqlCommand.Parameters.Add("@YearOfRelease", SqlDbType.NVarChar).Value = entity.Year;
			sqlCommand.Parameters.Add("@LicensExpirationDate", SqlDbType.NVarChar).Value = entity.LicensExpirationDate;
			sqlCommand.Parameters.Add("@DistributorId", SqlDbType.NVarChar).Value = entity.Distributor.Id;
			sqlCommand.Parameters.Add("@BasePrice", SqlDbType.NVarChar).Value = entity.BasePrice;
		}
	}
}
