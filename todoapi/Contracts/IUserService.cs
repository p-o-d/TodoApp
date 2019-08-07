using todoapi.Dtos;
using todoapi.Entities;

namespace todoapi.Contracts
{
    public interface IUserService
    {
         User Login(AuthCredentialsDto credentials);
    }
}