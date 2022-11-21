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
		private readonly IGetAllById<OneToMany<Review>> _reviewGetAllById;
		private readonly IGetAllById<OneToMany<Person>> _personGetAllById;
		private readonly IGetAllById<OneToMany<Score>> _scoreGetAllById;

		public FilmRepository(string connectionString, IRepository<Distributor> distributorRepository,
			IGetAllById<OneToMany<Review>> reviewGetAllById, IGetAllById<OneToMany<Person>> personGetAllById, IGetAllById<OneToMany<Score>> scoreGetAllById)
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
			_reviewGetAllById = reviewGetAllById;
			_personGetAllById = personGetAllById;
			_scoreGetAllById = scoreGetAllById;
		}

		public FilmRepository(string connectionString)
			: this(connectionString,
				 new DistributorRepository(connectionString),
				 new ReviewShortRepository(connectionString),
				 new PersonShortRepository(connectionString),
				 new ScoreShortRepository(connectionString))
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
