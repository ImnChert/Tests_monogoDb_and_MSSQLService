using ApplicationCore.Domain.Core.Models.Cinema.Films;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces;
using ApplicationCore.Domain.Interfaces.Interfaces;
using Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Data.MongoRepository.Implementations.GetAllByIdImplementations
{
	public class ScoreGetAllById : IGetAllById<Score>
	{
		private readonly IMongoCollection<BsonDocument> _mongoCollection;
		private IRepository<RegisteredUser> _registeredUserRepository;

		public ScoreGetAllById(string connectionString, IMongoCollection<BsonDocument> mongoCollection)
		{
			_mongoCollection = mongoCollection;
			_registeredUserRepository = new UserRepository(connectionString);
		}

		public async Task<List<Score>> GetAllByIdOneToMany(int id)
		{
			var pipeline = new BsonDocument
			{
				{"$unwind", "$scores"}
			};

			var pipeline2 = new BsonDocument
			{
				{
					"$match", new BsonDocument
					{
						{"_id", id }
					}
				}
			};

			var pipeline3 = new BsonDocument
			{
				{
					"$project", new BsonDocument
					{
						{"_id", "$scores._id" },
						{"username", "$scores.username"},
						{"registeredUser_id", "$scores.registeredUser_id"},
						{"score", "$scores.score"}
					}
				}
			};

			var pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
			List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

			var scores = new List<Score>();

			foreach (BsonDocument item in results)
			{
				scores.Add(new Score()
				{
					Id = item.GetValue("_id").ToInt32(),
					RegisteredUser = _registeredUserRepository.GetById(item.GetValue("registeredUser_id").ToInt32()).Result,
					Raiting = item.GetValue("score").ToInt32()
				});
			}

			return scores;
		}
	}
}
