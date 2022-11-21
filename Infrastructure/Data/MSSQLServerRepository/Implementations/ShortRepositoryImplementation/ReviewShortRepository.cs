using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Implementations.Repositories;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using Infrastructure.Data.MSSQLServerRepository.Connection.Extensions;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.ShortRepositoryImplementation
{
	public class ReviewShortRepository : MSSQLRepository<OneToMany<Review>>
	{
		private readonly IRepository<RegisteredUser> _registerUserRepository;

		public ReviewShortRepository(string connectionString, IRepository<RegisteredUser> registerUserRepository)
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
		}

		public ReviewShortRepository(string connectionString)
			: this(connectionString,
				  new UserRepository(connectionString))
		{
		}

		protected override OneToMany<Review> GetReader(SqlDataReader sqlDataReader)
			=> new OneToMany<Review>()
			{
				Value = new Review()
				{
					Id = (int)sqlDataReader["Id"],
					RegisteredUser = _registerUserRepository.GetById((int)sqlDataReader["RegisteredUsersId"]).Result,
					Description = sqlDataReader["Description"] as string ?? "Undefined"
				}
			};

		protected override void InsertCommand(SqlCommand sqlCommand, OneToMany<Review> entity)
		{
			sqlCommand.Parameters.Add("@RegisteredUsersId", SqlDbType.NVarChar).Value = entity.Value.RegisteredUser.Id;
			sqlCommand.Parameters.Add("@FilmId", SqlDbType.NVarChar).Value = entity.AdditionId;
			sqlCommand.Parameters.Add("@Description", SqlDbType.Decimal).Value = entity.Value.Description;
		}
	}
}
