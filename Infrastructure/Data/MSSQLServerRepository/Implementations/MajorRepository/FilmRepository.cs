using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class FilmRepository : MainMSSQLServerRepository<Film>
	{


		public FilmRepository(string connectionString, string tableName, string insertQuery, string updateQuery, string getAllQuery, string getByIdQuery)
			: base(connectionString,
				  "Films",
				  $@"INSERT INTO Films (Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice) 
					VALUES(@Id,@Name,@Duration,@DescriptionFilm,@YearOfRelease,@LicensExpirationDate,@DistributorId,@BasePrice)",
				  $@"UPDATE Films SET Name=@Name Duration=@Duration, DescriptionFilm=@DescriptionFilm, 
					YearOfRelease=@YearOfRelease, LicensExpirationDate=@LicensExpirationDate,
					DistributorId=@DistributorId, BasePrice=@BasePrice
					WHERE Id=@Id",
				  $"SELECT Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice FROM Films",
				  $"SELECT Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice FROM Films WHERE Id=@id")
		{
		}

		protected override Film GetReader(SqlDataReader sqlDataReader)
		=> new Film()
		{
			Id = (int)sqlDataReader["Id"],
			Name = sqlDataReader["Name"] as string ?? "Undefined", // TODO: в базе данных нету
			Duration = (int)sqlDataReader["Duration"],
			FilmCrew =
			Reviews =
			Description = sqlDataReader["Description"] as string ?? "Undefined",
			Year = (int)sqlDataReader["YearOfRelease"],
			Scores =
			LicensExpirationDate = DateTime.Parse((string)sqlDataReader["LicensExpirationDate"]),
			Distributor =
			BasePrice = (decimal)sqlDataReader["BasePrice"]
		};

		protected override void InsertCommand(SqlCommand sqlCommand, Film entity)
		{
			throw new NotImplementedException();
		}
	}
}
