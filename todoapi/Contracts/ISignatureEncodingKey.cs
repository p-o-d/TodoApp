using Microsoft.IdentityModel.Tokens;

namespace todoapi.Contracts
{
    public interface ISignatureEncodingKey
    {
         string Algorithm { get; }

         SecurityKey Key { get; }
    }
}