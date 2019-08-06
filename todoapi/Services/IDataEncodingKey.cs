using Microsoft.IdentityModel.Tokens;

namespace todoapi.Services
{
    public interface IDataEncodingKey
    {
         string SigningAlgorithm { get; }

         string EncryptingAlgorithm { get; }

         SecurityKey Key { get; }
    }
}