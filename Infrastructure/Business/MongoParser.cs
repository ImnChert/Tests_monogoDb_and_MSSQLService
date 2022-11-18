using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Business
{
	public class MongoParser
	{
		public int MaxIndex(IMongoCollection<BsonDocument> mongoCollection)
		{
			int maxValue = 0;
			var data = mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();

			if (data.Count > 0)
				maxValue = data[0].GetValue("_id").ToInt32();

			return maxValue;
		}
	}
}