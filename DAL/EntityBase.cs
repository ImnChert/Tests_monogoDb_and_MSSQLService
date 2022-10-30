using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models
{
	[Serializable]
	public abstract class EntityBase
	{
		[BsonId]
		public int Id { get; set; }
	}
}
