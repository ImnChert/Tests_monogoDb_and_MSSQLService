﻿using ApplicationCore.Domain.Core.Models.Roles.Staff;
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

		public List<Position> ParsePositions(BsonValue value)
		{
			var positions = new List<Position>();

			foreach (var item in value.AsBsonArray)
			{
				var position = item;
			}

			return positions;
		}
	}
}
