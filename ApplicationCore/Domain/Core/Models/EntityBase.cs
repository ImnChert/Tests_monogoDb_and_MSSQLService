using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCore.Domain.Core.Models
{
	public abstract class EntityBase
	{
		[BsonId]
		public int Id { get; set; }
	}
}
