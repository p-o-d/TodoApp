using Microsoft.IdentityModel.Tokens;

namespace todoapi.Services
{
    public interface ISignatureEncodingKey
    {
         string Algorithm { get; }

         SecurityKey Key { get; }
    }
}