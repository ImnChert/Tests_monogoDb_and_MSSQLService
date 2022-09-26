using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Observer;

namespace ApplicationCore.Models.Roles
{
    internal class RegisteredUser : Guest, IUser, IObserver
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        // TODO: пользоватлеь
    }
}
