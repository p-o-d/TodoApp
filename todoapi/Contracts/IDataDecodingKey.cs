using Microsoft.IdentityModel.Tokens;

namespace todoapi.Contracts
{
    public interface IDataDecodingKey
    {
         SecurityKey Key { get; }
    }
}