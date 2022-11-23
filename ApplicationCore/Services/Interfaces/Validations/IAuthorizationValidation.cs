using ApplicationCore.Domain.Core.Interfaces;

namespace ApplicationCore.Services.Interfaces.Validations
{
	public interface IAuthorizationValidation
	{
		public bool UsernameIsNotUse(string username, List<IAuthorization> authorizations);

		public bool UsernameIsValidate(string username);

		public bool PasswordIsValidate(string password);
	}
}
