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
			var data = mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();
			int maxValue = 0;

			data.ForEach(item => maxValue = item.GetValue("_id").ToInt32());

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
	}
}
