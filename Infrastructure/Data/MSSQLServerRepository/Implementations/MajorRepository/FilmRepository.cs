﻿using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Connection;
using System.Data.SqlClient;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository
{
	internal class FilmRepository : MainMSSQLServerRepository<Film>
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
				  $"SELECT Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice FROM Films",
				  $"SELECT Id,Name,Duration,DescriptionFilm,YearOfRelease,LicensExpirationDate,DistributorId,BasePrice FROM Films WHERE Id=@id")
		{
			_distributorRepository = distributorRepository;
			_reviewGetAllById = reviewGetAllById;
			_personGetAllById = personGetAllById;
			_scoreGetAllById = scoreGetAllById;
		}

		protected override Film GetReader(SqlDataReader sqlDataReader)
		=> new Film()
		{
			Id = (int)sqlDataReader["Id"],
			Name = sqlDataReader["Name"] as string ?? "Undefined", // TODO: в базе данных нету
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
			throw new NotImplementedException();
		}
	}
}
