using ApplicationCore.Services.Implementations.Repositories;
using Infrastructure.Data.MongoRepository.Implementations.RepositoryImplementetions;

namespace ConnectionTests
{
	public class DBConnectionTestMongo
	{
		[Fact]
		public async Task TestMongoDB_WithTheIncorrectPath_ThrowsException()
		{
			string connectionString = "path";
			int planeId = -1;

			try
			{
				var categoryRepository = new CategoryRepository(connectionString);
				var categoryService = new CategoryService(categoryRepository);

				var result = await categoryService.GetById(planeId);
			}
			catch (Exception ex)
			{
				Assert.True(true);
				return;
			}

			Assert.True(false);
		}

		[Fact]
		public async Task TestMongoDB_WithTheCorrectPath_True()
		{
			// Arrange
			string connectionString = "mongodb://localhost:27017";
			int planeId = -1;

			var categoryRepository = new CategoryRepository(connectionString);
			var categoryService = new CategoryService(categoryRepository);

			// Act
			DateTime startTime = DateTime.Now;
			var result = await categoryService.GetById(planeId);

			if (DateTime.Now.Subtract(startTime) >= new TimeSpan(0, 0, 0, 0, 10000))
			{
				Assert.True(false);
			}

			// Assert
			Assert.True(true);
		}
	}
}
