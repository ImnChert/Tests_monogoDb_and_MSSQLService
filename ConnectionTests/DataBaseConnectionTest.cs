using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Implementations.Repositories;
using Infrastructure.Data.MSSQLServerRepository.Implementations.MajorRepository;
using Microsoft.AspNetCore.Mvc;

namespace ConnectionTests
{
	public class DataBaseConnectionTest
	{
		[Fact]
		public async Task TestMSServer_WithTheIncorrectPath_ThrowsException()
		{
			// Arrange
			string connectionString = "path";
			int planeId = -1;
			var planeRepository = new CategoryRepository(connectionString);
			var planeService = new CategoryService(planeRepository);

			// Act
			var result = await planeService.GetById(planeId);

			// Assert
			Assert.Equal((new BadRequestResult()).StatusCode, result.StatusCode.StatusCode);
		}
	}
}
