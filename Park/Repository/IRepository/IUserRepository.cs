using Park.Models;
using System.Collections.Generic;

namespace Park.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Authenticate (string username, string password);
        User Register(string username, string password, string role);

    }
}
