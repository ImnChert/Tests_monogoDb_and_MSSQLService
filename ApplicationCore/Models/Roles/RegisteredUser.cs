using ApplicationCore.Interfaces;

namespace ApplicationCore.Models.Roles
{
    internal class RegisteredUser : Guest, IUser
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        // TODO: пользоватлеь
    }
}
