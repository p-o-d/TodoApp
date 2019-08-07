using Microsoft.IdentityModel.Tokens;

namespace todoapi.Contracts
{
    public interface ISignatureDecodingKey
    {
         SecurityKey Key { get; }
    }
}