using ApplicationCore.Services.Implementations.Repositories;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;

namespace ConnectionTests
{
	public class DBConnectionTestMSSQL
	{
		[Fact]
		public async Task TestMSServer_WithTheIncorrectPath_ThrowsException()
		{
			// Arrange
			string connectionString = "path";
			int planeId = -1;
			var categoryRepository = new CategoryRepository(connectionString);
			var categoryService = new CategoryRepositoryService(categoryRepository);

			// Act
			var result = await categoryService.GetById(planeId);

			// Assert
			Assert.Equal((new BadRequestResult()).StatusCode, result.StatusCode.StatusCode);
		}

		[Fact]
		public async Task TestMSServer_WithTheCorrectPath_True()
		{
			// Arrange
			string connectionString = @"Data Source=DESKTOP-CTBUCT0\SQLEXPRESS;;Initial Catalog=CP;Integrated Security=True";
			int planeId = -1;
			var categoryRepository = new CategoryRepository(connectionString);
			var categoryService = new CategoryRepositoryService(categoryRepository);

			// Act
			DateTime startTime = DateTime.Now;
			await categoryService.GetById(planeId);

			if (DateTime.Now.Subtract(startTime) >= new TimeSpan(0, 0, 0, 0, 10000))
			{
				Assert.True(false);
			}

			// Assert
			Assert.True(true);
		}

	}
}
