using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles.Staff;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Business
{
	internal class MongoParser
	{
		public int MaxIndex(IMongoCollection<BsonDocument> mongoCollection)
		{
			int maxValue = 0;
			var data = mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();

			if (data.Count > 0)
				maxValue = data[0].GetValue("_id").ToInt32();

			return maxValue;
		}

		// TODO: сделать парсеры
		public List<Position> ParsePositions(BsonValue value)
		{
			var positions = new List<Position>();

			foreach (var item in value.AsBsonArray)
			{
				var position = item;
			}

			return positions;
		}

		public List<Person> ParsePersons(BsonValue value)
		{
			return null;
		}

		public List<Review> ParseReviews(BsonValue value)
		{
			return null;
		}

		public List<Score> ParseScores(BsonValue value)
		{
			return null;
		}

		public List<Session> ParseSessions(BsonValue value)
		{
			return null;
		}
	}
}
