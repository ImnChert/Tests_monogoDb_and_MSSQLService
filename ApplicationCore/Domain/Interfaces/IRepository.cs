using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces.Interfaces
{
	public interface IRepository<T> : IShortRepository<T> where T : EntityBase
	{
		public Task<List<T>> GetAllAsync();
		public Task<T> GetById(int id);
	}
}
