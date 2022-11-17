using ApplicationCore.Domain.Core.Models.Cinema;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace Infrastructure.Data.MongoRepository.Implementations
{
	public class ScheduleRepository
		: MainMongoRepository<Schedule>
	{
		public ScheduleRepository(string connectionString)
			: base(connectionString, "schedule")
		{
		}

		public override async Task<List<Schedule>> GetAllAsync()
		{
			var filter = new BsonDocument();
			var schedules = new List<Schedule>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				var parse = new MongoParser();
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> user = cursor.Current;

					foreach (BsonDocument item in user)
					{
						schedules.Add(InitializationSchedule(item, parse));
					}
				}
			}

			return schedules;
		}

		public override async Task<Schedule> GetById(int id)
		{
			var schedule = new Schedule();
			var filter = new BsonDocument("_id", id);

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				if (await cursor.MoveNextAsync())
				{
					if (cursor.Current.Count() == 0)
						return null;

					var elements = cursor.Current.ToList();
					BsonDocument item = elements[0];

					var parse = new MongoParser();
					schedule = InitializationSchedule(item, parse);
				}
			}

			return schedule;
		}

		public Schedule InitializationSchedule(BsonDocument item, MongoParser parse) => new Schedule()
		{
			Id = item.GetValue("_id").ToInt32(),
			Date = DateTime.Parse(item.GetValue("date").ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal),
			Hall = new Hall()
			{
				Number = item.GetValue("numberHall").ToInt32(),
			},
			Sessions = parse.ParseSessions(item.GetValue("sessions"))
		};

		public override async Task<bool> InsertAsync(Schedule entity)
		{
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var sessions = new BsonDocument();

			entity.Sessions.ForEach(item =>
			{
				sessions.AddRange(new BsonDocument
				{
					{"_id", item.Id},
					{"nameFilm", item.Film.Name},
					{"duration", item.Film.Duration},
					{"basePrice", item.Film.BasePrice},
					{"film_id", item.Film.Id},
					{"start", item.StartTime}
				});
			});

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"numberHall",entity.Hall.Number },
				{"date",entity.Date },
				{"sessions", sessions },

			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

		public override async Task<bool> UpdateAsync(Schedule entity)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("numberHall", entity.Hall.Number);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("date", entity.Date);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("sessions", entity.Sessions);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
	}
}
