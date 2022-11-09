using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class CategoryService : IRepositoryBaseResponse<Category>
	{
		private readonly IRepository<Category> _categoryRepository;

		public CategoryService(IRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<BaseResponse<bool>> DeleteAsync(Category entity)
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

				await _categoryRepository.DeleteAsync(entity);

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

		public async Task<BaseResponse<List<Category>>> GetAllAsync()
		{
			try
			{
				List<Category> categories = await _categoryRepository.GetAllAsync();

				return new BaseResponse<List<Category>>()
				{
					Data = categories,
					Description = "Data received successfully.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Category>>()
				{
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<Category>> GetById(int id)
		{
			try
			{

				if ((Category?)await _categoryRepository.GetById(id) == null)
				{
					return new BaseResponse<Category>()
					{
						StatusCode = new BadRequestResult(),
						Description = "A user with this id doesn't exist"
					};
				}

				return new BaseResponse<Category>()
				{
					Data = (Category?)await _categoryRepository.GetById(id),
					Description = "The user was successfully found.",
					StatusCode = new BadRequestResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Category>()
				{
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<bool>> InsertAsync(Category entity)
		{
			try
			{
				Category category = await _categoryRepository.GetById(entity.Id);

				if (category != null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new BadRequestResult(),
						Description = "A category with this name already exists"
					};
				}

				await _categoryRepository.InsertAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					Description = "The category was successfully added.",
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

		public async Task<BaseResponse<bool>> UpdateAsync(Category entity)
		{
			try
			{
				Task<Category> data = _categoryRepository.GetById(entity.Id);

				if (data == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new BadRequestResult(),
						Description = "A user with this id doesn't exist"
					};
				}

				await _categoryRepository.UpdateAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					Description = "The user information was successfully updates.",
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
