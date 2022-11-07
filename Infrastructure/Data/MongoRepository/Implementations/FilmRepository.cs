using ApplicationCore.Domain.Core.Models.Cinema.Films;
using Infrastructure.Business;
using Infrastructure.Data.MongoRepository.Connection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations
{
    internal class FilmRepository : MainMongoRepository<Film>
    {
        public FilmRepository(string connectionString)
            : base(connectionString, "films")
        {
        }

        public override async Task<List<Film>> GetAllAsync()
        {
			var filter = new BsonDocument();
			var films = new List<Film>();

			using (IAsyncCursor<BsonDocument> cursor = await _mongoCollection.FindAsync(filter))
			{
				var parse = new MongoParser();
				while (await cursor.MoveNextAsync())
				{
					IEnumerable<BsonDocument> user = cursor.Current;

					foreach (BsonDocument item in user)
					{
						films.Add(InitializationFilm(item, parse));
					}
				}
			}

			return films;
		}

        public override async Task<Film> GetById(int id)
        {
			var film = new Film();
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
					film = InitializationFilm(item, parse);
				}
			}

			return film;
		}

		public Film InitializationFilm(BsonDocument item, MongoParser parse) => new Film()
		{
			Id = item.GetValue("_id").ToInt32(),
			Name = item.GetValue("name").ToString(),
			Duration = item.GetValue("duration").ToInt32(),
			Description = item.GetValue("description").ToString(),
			FilmCrew = parse.ParsePersons(item.GetValue("staff")),
			Reviews = parse.ParseReviews(item.GetValue("rewiews")),
			Scores = parse.ParseScores(item.GetValue("scores")),
			Year = item.GetValue("yearOfRelease").ToInt32(),
			Distributor = new Distributor()
			{
				NameCompany = item.GetValue("companyName").ToString(),
				Id = item.GetValue("distributor_id").ToInt32(),
			},
			BasePrice = item.GetValue("basePrice").ToInt32(),
			LicensExpirationDate = DateTime.Parse((string)item.GetValue("licensExpirationDate"))
		};

		public override async Task<bool> InsertAsync(Film entity)
        {
			var parser = new MongoParser();
			entity.Id = parser.MaxIndex(_mongoCollection) + 1;

			var staff = new BsonDocument();

			entity.FilmCrew.ForEach(item =>
			{
				staff.AddRange(new BsonDocument
				{
					{"firstName", item.FirstName},
					{"lastName", item.LastName},
					{"middleName", item.MiddleName},
					{"post", item.Post},
					{"staff_id", item.Id}
				});
			});

			var rewiews = new BsonDocument();

			entity.Reviews.ForEach(item =>
			{
				rewiews.AddRange(new BsonDocument
				{
					{"username", item.RegisteredUser.Username},
					{"registeredUser_id", item.RegisteredUser.Id},
					{"discription", item.Description}
				});
			});

			var score = new BsonDocument();

			entity.Scores.ForEach(item =>
			{
				rewiews.AddRange(new BsonDocument
				{
					{"username", item.RegisteredUser.Username},
					{"registeredUser_id", item.RegisteredUser.Id},
					{"score", item.Raiting}
				});
			});

			var document = new BsonDocument
			{
				{ "_id", entity.Id },
				{"name",entity.Name },
				{"duration",entity.Duration },
				{"description",entity.Description },
				{"staff", staff },
				{"reviews", rewiews },
				{"score", score },
				{"companyName",entity.Distributor.NameCompany },
				{"distributor_id",entity.Distributor.Id },
				{"basePrice",entity.BasePrice },
				{"licensExpirationDate",entity.LicensExpirationDate }
			};

			await _mongoCollection.InsertOneAsync(document);

			return true;
		}

        public override async Task<bool> UpdateAsync(Film entity)
        {
			var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

			var update = Builders<BsonDocument>.Update.Set("name", entity.Name);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("duration", entity.Duration);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("description", entity.Description);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("staff", entity.FilmCrew);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("reviews", entity.Reviews);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("score", entity.Scores);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("year", entity.Year);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("companyName", entity.Distributor.NameCompany);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("basePrice", entity.BasePrice);
			await _mongoCollection.UpdateOneAsync(filter, update);

			update = Builders<BsonDocument>.Update.Set("licensExpirationDate", entity.LicensExpirationDate);
			await _mongoCollection.UpdateOneAsync(filter, update);

			return true;
		}
    }
}
