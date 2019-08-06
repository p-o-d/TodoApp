using Microsoft.IdentityModel.Tokens;

namespace todoapi.Services
{
    public interface ISignatureDecodingKey
    {
         SecurityKey Key { get; }
    }
}