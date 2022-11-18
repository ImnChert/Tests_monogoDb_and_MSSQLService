using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations
{
	public class SessionGetAllById : IGetAllById<Session>
	{
		IMongoCollection<BsonDocument> _mongoCollection;
		private IRepository<Film> _filmRepository;
		private IGetAllById<Ticket> _ticketGetAllByID;

		public SessionGetAllById(string connectionString, IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
			_filmRepository = new FilmRepository(connectionString);
			_ticketGetAllByID = new TicketGetAllById(connectionString, mongoCollection);
		}

		public async Task<List<Session>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument
			{
				{"$unwind", "$sessions"}
			};

			var pipeline2 = new BsonDocument
			{
				{"$match", new BsonDocument{
					{"_id", id }
				}}
			};

			var pipeline3 = new BsonDocument
			{
				{
					"$project", new BsonDocument
					{
						{ "_id", "$_id"},
						{"nameFilm", "$sessions.nameFilm"},
						{"duration", "$sessions.duration"},
						{"basePrice", "$sessions.basePrice"},
						{"film_id",  "$sessions.film_id"},
						{"start", "$sessions.start"}
					}
				}
			};

			BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
			List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

			List<Session> sessions = new();

			foreach (BsonDocument item in results)
			{
				sessions.Add(new Session()
				{
					Id = item.GetValue("_id").ToInt32(),
					Film = _filmRepository.GetById(item.GetValue("film_id").ToInt32()).Result,
					StartTime = DateTime.Parse(item.GetValue("start").ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
					Tickets = _ticketGetAllByID.GetAllByIdOneToMany(item.GetValue("_id").ToInt32()).Result
				});
			}

			return sessions;
		}
	}
}
