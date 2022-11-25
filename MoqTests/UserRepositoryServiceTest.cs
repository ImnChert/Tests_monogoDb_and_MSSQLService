using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Implementations.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MoqTests
{
	public class UserRepositoryServiceTest
	{
		[Fact]
		public async Task InsertAsync_InsertNewUser_True()
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
		public async Task GetAllAsync_GetAllUsers_Equal()
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

		[Fact]
		public async Task GetById_GetUsersById_Equal()
		{
			// Arrange
			var collectionUsers = GetUsers();
			int userId = collectionUsers[0].Id;

			var repositoryMock = new Mock<IRepository<RegisteredUser>>();
			repositoryMock.Setup(repo => repo.GetById(userId))
				.ReturnsAsync(new RegisteredUser() { Id = 1 });

			var service = new UserRepositoryService(repositoryMock.Object);

			// Act
			var result = await service.GetById(userId);

			// Assert
			Assert.Equal(userId, result.Data.Id);
		}

		[Fact]
		public async Task GetById_SetAnIdThatDoesnNotExist_NotEqual()
		{
			// Arrange
			var collectionUsers = GetUsers();

			int planeId = collectionUsers[0].Id;
			int incorrectId = collectionUsers.Max(i => i.Id) + 1;

			var repositoryMock = new Mock<IRepository<RegisteredUser>>();
			repositoryMock.Setup(repo => repo.GetById(planeId))
				.ReturnsAsync(new RegisteredUser() { Id = 1 });

			var service = new UserRepositoryService(repositoryMock.Object);

			// Act
			var result = await service.GetById(incorrectId);

			// Assert
			Assert.NotEqual(new OkResult(), result.StatusCode);
		}

		[Fact]
		public async Task DeleteAsync_DeletingAUserWithASpecifiedId_True()
		{
			// Arrange
			var collectionUsers = GetUsers();
			RegisteredUser user = collectionUsers[0]; //1

			var repositoryMock = new Mock<IRepository<RegisteredUser>>();
			repositoryMock.Setup(repo => repo.DeleteAsync(user))
					.ReturnsAsync(true);

			var service = new UserRepositoryService(repositoryMock.Object);

			// Act
			var result = await service.DeleteAsync(user);

			// Assert
			Assert.True(result.Data);
		}

		[Fact]
		public async Task UpdateAsync_UpdateUser_True()
		{
			// Arrange
			var collectionUsers = GetUsers();
			RegisteredUser user = collectionUsers[0]; //1

			var repositoryMock = new Mock<IRepository<RegisteredUser>>();
			repositoryMock.Setup(repo => repo.UpdateAsync(user))
					.ReturnsAsync(true);

			var service = new UserRepositoryService(repositoryMock.Object);

			// Act
			var result = await service.UpdateAsync(user);

			// Assert
			Assert.True(result.Data);
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
