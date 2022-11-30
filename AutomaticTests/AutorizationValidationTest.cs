using ApplicationCore.Domain.Core.Interfaces;
using ApplicationCore.Domain.Core.Models.Roles;
using ApplicationCore.Services.Implementations.Validations;

namespace AutomaticTests
{
	public class AutorizationValidationTest
	{
		[Fact]
		public void UsernameIsNotUse_GettingValidationSet_True()
		{
			// Arange
			var collectionUser = GetValidSet();
			string username = "Богдан";
			var autorization = new AutorizationValidation();

			// Act
			var result = autorization.UsernameIsNotUse(username, collectionUser);

			//Assert

			Assert.True(result);
		}

		[Fact]
		public void UsernameIsNotUse_GettingDoNotValidationSet_False()
		{
			// Arange
			var collectionUser = GetValidSet();
			string username = "Богдан228";
			var autorization = new AutorizationValidation();

			// Act
			var result = autorization.UsernameIsNotUse(username, collectionUser);

			//Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("dsdhh22323")]
		[InlineData("ddssdds231")]
		[InlineData("sass12233")]
		public void UsernameIsValidate_GettingValidationSet_True(string username)
		{
			// Arange
			var autorization = new AutorizationValidation();

			//Act
			var result = autorization.UsernameIsValidate(username);

			//Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData("ssssssssssssssssssss")]
		[InlineData("111111111111111111111")]
		[InlineData("23s")]
		[InlineData("1211s22222222dsdd")]
		public void UsernameIsValidate_GettingDoNotValidationSet_False(string username)
		{
			// Arange
			var autorization = new AutorizationValidation();

			//Act
			var result = autorization.UsernameIsValidate(username);

			//Assert
			Assert.False(result);
		}

		[Theory]
		[InlineData("232323121@ndns")]
		[InlineData("2313123@djsdjsjds")]
		[InlineData("sdadsd@hdjsjd21")]
		public void PasswordIsValidate_GettingValidationSet_True(string username)
		{
			// Arange
			var autorization = new AutorizationValidation();

			//Act
			var result = autorization.PasswordIsValidate(username);

			//Assert
			Assert.True(result);
		}

		[Theory]
		[InlineData("halslsls")]
		[InlineData("dsdsdsds")]
		[InlineData("ddosdosdo")]
		[InlineData("1234")]
		public void PasswordIsValidate_GettingDoNotValidationSet_False(string username)
		{
			// Arange
			var autorization = new AutorizationValidation();

			//Act
			var result = autorization.PasswordIsValidate(username);

			//Assert
			Assert.False(result);
		}

		private List<IAuthorization> GetValidSet()
			=> new List<IAuthorization>()
			{
				new RegisteredUser()
				{
					Username = "Богдан228"
				},
				new RegisteredUser()
				{
					Username = "Богдан8"
				},
				new RegisteredUser()
				{
					Username = "Богд28"
				},
				new RegisteredUser()
				{
					Username = "Богдан2"
				}
			};
	}
}
