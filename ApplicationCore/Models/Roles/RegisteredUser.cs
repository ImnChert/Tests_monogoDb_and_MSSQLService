using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Observer;

namespace ApplicationCore.Models.Roles
{
    internal class RegisteredUser : Guest, IUser, IObserver, IAuthorization
	{
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Username { get; set; }
        public string Password { get; set; }
        // TODO: пользоватлеь
    }
}
