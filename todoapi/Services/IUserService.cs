using todoapi.Dtos;
using todoapi.Entities;

namespace todoapi.Services
{
    public interface IUserService
    {
         User Login(AuthCredentialsDto credentials);
    }
}