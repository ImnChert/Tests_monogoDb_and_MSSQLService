using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	public class ReviewRepository : MSSQLFullRepository<Review>
	{
		private readonly Film _film;
		private readonly IRepository<RegisteredUser> _registerUserRepository;

		public ReviewRepository(string connectionString, IRepository<RegisteredUser> registerUserRepository, Film film)
			: base(connectionString,
				  "Reviews",
				  $@"INSERT INTO Reviews (Id,FilmId,RegisteredUsersId,DescriptionReview) 
					VALUES(@Id,@FilmId,@RegisteredUsersId,@DescriptionReview)",
				  $@"UPDATE Reviews SET FilmId=@FilmId, RegisteredUsersId=@RegisteredUsersId, DescriptionReview=@DescriptionReview 
					WHERE Id=@Id",
				  $"SELECT Id,FilmId,RegisteredUsersId,DescriptionReview FROM Reviews",
				  $"SELECT Id,FilmId,RegisteredUsersId,DescriptionReview FROM Reviews WHERE Id=@id",
				  $"SELECT Id,FilmId,RegisteredUsersId,DescriptionReview FROM Reviews WHERE FilmsId=@FilmsId",
				  "@FilmsId")
		{
			_registerUserRepository = registerUserRepository;
			_film = film;
		}

		protected override Review GetReader(SqlDataReader sqlDataReader)
			=> new Review()
			{
				Id = (int)sqlDataReader["Id"],
				RegisteredUser = _registerUserRepository.GetById((int)sqlDataReader["RegisteredUsersId"]).Result,
				Description = sqlDataReader["Description"] as string ?? "Undefined"
			};

		protected override void InsertCommand(SqlCommand sqlCommand, Review entity)
		{
			sqlCommand.Parameters.Add("@RegisteredUsersId", SqlDbType.NVarChar).Value = entity.RegisteredUser.Id;
			sqlCommand.Parameters.Add("@FilmId", SqlDbType.NVarChar).Value = _film.Id;
			sqlCommand.Parameters.Add("@Description", SqlDbType.Decimal).Value = entity.Description;
		}
	}
}
