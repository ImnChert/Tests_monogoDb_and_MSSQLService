using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Implementations.Repositories;

namespace MoqTests
{
	public class UserRepositoryServiceTest
	{
		[Fact]
		public async Task InsertAsync_InsertNewUser_IsTrue()
		{
			// Arange 

			var newUser = new RegisteredUser()
			{
				Username = "test",
				Password = "test",
				FirstName = "Nikita",
				MiddleName = "Valerievich",
				LastName = "Tishkov",
				DateOfBirthday = DateTime.Now,
				Phone = "+375257595849"
			};

			var repositoryMock = new Mock<IRepository<RegisteredUser>>();
			repositoryMock.Setup(repo => repo.InsertAsync(newUser))
				.ReturnsAsync(true);

			var service = new UserRepositoryService(repositoryMock.Object);

			// Act

			var result = await service.InsertAsync(newUser);

			//Assert

			Assert.True(result.Data);
		}

		[Fact]
		public async Task GetTaskAsync_GetAllUsers_IsTrue()
		{
			// Arrange
			var collectionUsers = GetUsers();
			int count = collectionUsers.Count;

			var repositoryMock = new Mock<IRepository<RegisteredUser>>();
			repositoryMock.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(collectionUsers);

			var service = new UserRepositoryService(repositoryMock.Object);

			// Act
			var result = await service.GetAllAsync();

			// Assert
			Assert.Equal(count, result.Data.Count);
		}

		private List<RegisteredUser> GetUsers()
			=> new List<RegisteredUser>()
			{
				new RegisteredUser()
				{
					Id = 1,
					Username = "test1",
					Password = "test",
					FirstName = "Nikita",
					MiddleName = "Valerievich",
					LastName = "Tishkov",
					DateOfBirthday = DateTime.Now,
					Phone = "+375257595849"
				},
				new RegisteredUser()
				{
					Id = 3,
					Username = "test3",
					Password = "test",
					FirstName = "Nikita",
					MiddleName = "Valerievich",
					LastName = "Tishkov",
					DateOfBirthday = DateTime.Now,
					Phone = "+375257595841"
				},
				new RegisteredUser()
				{
					Id = 3,
					Username = "test2",
					Password = "test",
					FirstName = "Nikita",
					MiddleName = "Valerievich",
					LastName = "Tishkov",
					DateOfBirthday = DateTime.Now,
					Phone = "+375257595843"
				}
			};
	}
}
