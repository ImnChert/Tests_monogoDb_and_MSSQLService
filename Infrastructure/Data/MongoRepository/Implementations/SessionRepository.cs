using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations
{
	internal class SessionRepository : MainMongoRepository<Session>
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

		public override Task<bool> InsertAsync(Session entity)
		{
			throw new NotImplementedException();
		}

		public override Task<bool> UpdateAsync(Session entity)
		{
			throw new NotImplementedException();
		}
	}
}
