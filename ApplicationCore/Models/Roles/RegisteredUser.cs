using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Observer;
using ApplicationCore.Models.Cinema;

namespace ApplicationCore.Models.Roles
{
    internal class RegisteredUser : Guest, IUser, IObserver, IAuthorization
	{
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
