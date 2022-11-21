using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using Infrastructure.Data.MSSQLServerRepository.Implementations.ShortRepositoryImplementation;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class FilmRepository : MSSQLRepository<Film>
	{
		private IRepository<Distributor> _distributorRepository;
		private readonly IRepository<OneToMany<Review>> _reviewRepository;
		private readonly IRepository<OneToMany<Person>> _personRepository;
		private readonly IRepository<OneToMany<Score>> _scoreRepository;
		private readonly IGetAllById<OneToMany<Review>> _reviewGetAllById;
		private readonly IGetAllById<OneToMany<Person>> _personGetAllById;
		private readonly IGetAllById<OneToMany<Score>> _scoreGetAllById;

		public FilmRepository(string connectionString,
			IRepository<Distributor> distributorRepository,
			IRepository<OneToMany<Review>> reviewRepository,
			IRepository<OneToMany<Person>> personRepository,
			IRepository<OneToMany<Score>> scoreRepository,
			IGetAllById<OneToMany<Review>> reviewGetAllById,
			IGetAllById<OneToMany<Person>> personGetAllById,
			IGetAllById<OneToMany<Score>> scoreGetAllById)
			: base(connectionString,
				  "Films",
				  $@"INSERT INTO Films (Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice) 
					VALUES(@Id,@Name,@Duration,@DescriptionFilm,@YearOfRelease,@LicensExpirationDate,@DistributorId,@BasePrice)",
				  $@"UPDATE Films SET Name=@Name Duration=@Duration, DescriptionFilm=@DescriptionFilm, 
					YearOfRelease=@YearOfRelease, LicensExpirationDate=@LicensExpirationDate,
					DistributorId=@DistributorId, BasePrice=@BasePrice
					WHERE Id=@Id",
				  $@"SELECT Films.Id, Name, Duration, DescriptionFilm, YearOfRelease, LicensExpirationDate, DistributorId, BasePrice
					FROM Films",
				  $@"SELECT Films.Id, Name, Duration, DescriptionFilm, YearOfRelease, LicensExpirationDate, DistributorId, BasePrice
					FROM Films
					WHERE Id=@id")
		{
			_distributorRepository = distributorRepository;
			_reviewRepository = reviewRepository;
			_scoreRepository = scoreRepository;
			_personRepository = personRepository;
			_reviewGetAllById = reviewGetAllById;
			_personGetAllById = personGetAllById;
			_scoreGetAllById = scoreGetAllById;
		}

		public FilmRepository(string connectionString)
			: this(connectionString,
				 new DistributorRepository(connectionString),
				 new ReviewOneToManyRepository(connectionString),
				 new PersonOneToManyRepository(connectionString),
				 new ScoreOneToManyRepository(connectionString),
				 new ReviewOneToManyRepository(connectionString),
				 new PersonOneToManyRepository(connectionString),
				 new ScoreOneToManyRepository(connectionString))
		{
		}

		protected override Film GetReader(SqlDataReader sqlDataReader)
			=> new Film()
			{
				Id = (int)sqlDataReader["Id"],
				Name = sqlDataReader["Name"] as string ?? "Undefined",
				Duration = (int)sqlDataReader["Duration"],
				FilmCrew = _personGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result.Select(x => x.Value).ToList(),
				Reviews = _reviewGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result.Select(x => x.Value).ToList(),
				Description = sqlDataReader["Description"] as string ?? "Undefined",
				Year = (int)sqlDataReader["YearOfRelease"],
				Scores = _scoreGetAllById.GetAllByIdOneToMany((int)sqlDataReader["Id"]).Result.Select(x => x.Value).ToList(),
				LicensExpirationDate = DateTime.Parse((string)sqlDataReader["LicensExpirationDate"]),
				Distributor = _distributorRepository.GetById((int)sqlDataReader["DistributorId"]).Result,
				BasePrice = (decimal)sqlDataReader["BasePrice"]
			};

		protected override sealed void InsertCommand(SqlCommand sqlCommand, Film entity)
		{
			sqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
			sqlCommand.Parameters.Add("@Duration", SqlDbType.NVarChar).Value = entity.Duration;
			sqlCommand.Parameters.Add("@DescriptionFilm", SqlDbType.NVarChar).Value = entity.Description;
			sqlCommand.Parameters.Add("@YearOfRelease", SqlDbType.NVarChar).Value = entity.Year;
			sqlCommand.Parameters.Add("@LicensExpirationDate", SqlDbType.NVarChar).Value = entity.LicensExpirationDate;
			sqlCommand.Parameters.Add("@DistributorId", SqlDbType.NVarChar).Value = entity.Distributor.Id;
			sqlCommand.Parameters.Add("@BasePrice", SqlDbType.NVarChar).Value = entity.BasePrice;
		}

		protected override async Task<bool> InsertSqlCommand(SqlCommand sqlCommand, Film entity)
		{
			InsertCommand(sqlCommand, entity);

			entity.Scores.ForEach(score => _scoreRepository.InsertAsync(new OneToMany<Score>() { AdditionId = entity.Id, Value = score }));
			entity.Reviews.ForEach(review => _reviewRepository.InsertAsync(new OneToMany<Review>() { AdditionId = entity.Id, Value = review }));
			entity.FilmCrew.ForEach(person => _personRepository.InsertAsync(new OneToMany<Person>() { AdditionId = entity.Id, Value = person }));

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}

		protected override async Task<bool> UpdateSqlCommand(SqlCommand sqlCommand, Film entity)
		{
			sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
			InsertCommand(sqlCommand, entity);

			entity.Scores.ForEach(score => _scoreRepository.UpdateAsync(new OneToMany<Score>() { AdditionId = entity.Id, Value = score }));
			entity.Reviews.ForEach(review => _reviewRepository.UpdateAsync(new OneToMany<Review>() { AdditionId = entity.Id, Value = review }));
			entity.FilmCrew.ForEach(person => _personRepository.UpdateAsync(new OneToMany<Person>() { AdditionId = entity.Id, Value = person }));

			await sqlCommand.ExecuteNonQueryAsync();

			return true;
		}
	}
}
