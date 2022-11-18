﻿using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Core.Models.Roles.Staff.Positions;
using ApplicationCore.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations
{
	internal class PositionGetAllById : IGetAllById<Position>
	{
		private readonly IMongoCollection<BsonDocument> _mongoCollection;

		public PositionGetAllById(IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
		}

		private Position GetType(int id, string name)
		{
			if (name == "Admin")
				return new Admin() { Id = id, Name = name };

			return null;
		}

		public async Task<List<Position>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument
			{
				{"$unwind", "$post"}
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
						{ "_id", "$post.post_id" },
						{ "name", "$post.name"}
					}
				}
			};

			BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
			List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

			List<Position> positions = new();

			foreach (BsonDocument item in results)
			{
				Position position = GetType(item.GetValue("_id").ToInt32(), item.GetValue("name").ToString());
				positions.Add(position);
			}

			return positions;
		}
	}
}
