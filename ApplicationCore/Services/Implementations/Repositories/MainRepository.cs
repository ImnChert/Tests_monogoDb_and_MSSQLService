using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public abstract class MainRepository<T>
		: IRepositoryBaseResponse<T> where T : EntityBase
	{
		private readonly IRepository<T> _repository;

		protected MainRepository(IRepository<T> repository)
		{
			_repository = repository;
		}

		public async Task<BaseResponse<bool>> DeleteAsync(T entity)
		{
			try
			{
				if (entity == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new NotFoundResult(),
						Description = "Null object"
					};
				}

				await _repository.DeleteAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					StatusCode = new OkResult(),
					Description = "Ok result"
				};
			}
			catch
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					StatusCode = new BadRequestResult(),
					Description = "Inevitable error"
				};
			}
		}

		public async Task<BaseResponse<List<T>>> GetAllAsync()
		{
			try
			{
				List<T> values = await _repository.GetAllAsync();

				return new BaseResponse<List<T>>()
				{
					Data = values,
					Description = "Data received successfully.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<T>>()
				{
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<T>> GetById(int id)
		{
			try
			{
				if ((T?)await _repository.GetById(id) == null)
				{
					return new BaseResponse<T>()
					{
						StatusCode = new BadRequestResult(),
						Description = "A value with this id doesn't exist"
					};
				}

				return new BaseResponse<T>()
				{
					Data = (T?)await _repository.GetById(id),
					Description = "The value was successfully found.",
					StatusCode = new BadRequestResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<T>()
				{
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<bool>> InsertAsync(T entity)
		{
			try
			{
				T value = await _repository.GetById(entity.Id);

				if (value != null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new BadRequestResult(),
						Description = "A value with this name already exists"
					};
				}

				await _repository.InsertAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					Description = "The value was successfully added.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<bool>> UpdateAsync(T entity)
		{
			try
			{
				Task<T> value = _repository.GetById(entity.Id);

				if (value == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new BadRequestResult(),
						Description = "A value with this id doesn't exist"
					};
				}

				await _repository.UpdateAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					Description = "The value information was successfully updates.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}
	}
}
