using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations
{
	public class ReviewGetAllById : IGetAllById<Review>
	{
		private readonly IMongoCollection<BsonDocument> _mongoCollection;
		private IRepository<RegisteredUser> _registeredUserRepository;

		public ReviewGetAllById(string connectionString, IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
			_registeredUserRepository = new UserRepository(connectionString);
		}

		public async Task<List<Review>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument
			{
				{"$unwind", "$reviews"}
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
						{"_id", "$reviews._id" },
						{"username", "$reviews.username"},
						{"registeredUser_id", "$reviews.registeredUser_id"},
						{"discription", "$reviews.discription"}
					}
				}
			};

			var pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
			List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

			var reviews = new List<Review>();

			foreach (BsonDocument item in results)
			{
				reviews.Add(new Review()
				{
					Id = item.GetValue("_id").ToInt32(),
					RegisteredUser = _registeredUserRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,
					Description = item.GetValue("discription").ToString() as string ?? "Undefined"
				});
			}

			return reviews;
		}
	}
}
