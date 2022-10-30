using ApplicationCore.Domain.Core.Interfaces;
using ApplicationCore.Domain.Core.Interfaces.Observer;

namespace ApplicationCore.Domain.Core.Models.Roles
{
	public class RegisteredUser : Guest, IObserver, IAuthorization
	{
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisteredUser(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public void Update(ISubject subject, Exception exception)
        {
             
            throw exception;
            
        }
        // TODO: пользоватлеь
    }
}
