using Microsoft.IdentityModel.Tokens;

namespace todoapi.Contracts
{
    public interface IDataEncodingKey
    {
         string SigningAlgorithm { get; }

         string EncryptingAlgorithm { get; }

         SecurityKey Key { get; }
    }
}