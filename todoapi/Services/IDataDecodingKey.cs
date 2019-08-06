using Microsoft.IdentityModel.Tokens;

namespace todoapi.Services
{
    public interface IDataDecodingKey
    {
         SecurityKey Key { get; }
    }
}