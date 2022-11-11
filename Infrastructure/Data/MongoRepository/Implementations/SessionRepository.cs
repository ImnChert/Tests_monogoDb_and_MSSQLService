using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations
{
	public class SessionRepository : MainMongoRepository<Session>
	{
		private IRepository<Film> _filmRepository;

		public SessionRepository(string connectionString) : base(connectionString, "sessions")
		{
			_filmRepository = new FilmRepository(connectionString);
		}

		public override async Task<List<Session>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var sessions = new List<Session>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> user = cursor.Current;

					foreach (BsonDocument item in user)
					{
						sessions.Add(InitializationSession(item));
					}
				}
			}

			return sessions;
		}

		public override async Task<Session> GetById(int id)
		{
			var session = new Session();
			var filter = new BsonDocument("_id", id);

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				if (await cursor.MoveNextAsync())
				{
					if (cursor.Current.Count() == 0)
						return null;

					var elements = cursor.Current.ToList();
					BsonDocument item = elements[0];

					session = InitializationSession(item);
				}
			}

			return session;
		}

		public Session InitializationSession(BsonDocument item) => new Session()
		{
			Id = item.GetValue("_id").ToInt32(),
			Film = _filmRepository.GetById(item.GetValue("film_id").ToInt32()).Result,
			StartTime = DateTime.Parse((string)item.GetValue("start")),
		};

		public override async Task<bool> InsertAsync(Session entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var document = new BsonDocument
			{
				{"_id", entity.Id},
				{"nameFilm", entity.Film.Name},
				{"duration", entity.Film.Duration},
				{"basePrice", entity.Film.BasePrice},
				{"film_id", entity.Film.Id},
				{"start", entity.StartTime}

			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(Session entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("nameFilm", entity.Film.Name);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("duration", entity.Film.Duration);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("basePrice", entity.Film.BasePrice);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("film_id", entity.Film.Id);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("start", entity.StartTime);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}
