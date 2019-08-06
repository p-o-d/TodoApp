using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace todoapi.Services.Impl
{
    public class SymmetricDataKey : IDataEncodingKey, IDataDecodingKey
    {
        public string SigningAlgorithm => JwtConstants.DirectKeyUseAlg;

        public string EncryptingAlgorithm => SecurityAlgorithms.Aes256CbcHmacSha512;

        public SecurityKey Key { get; }

        public SymmetricDataKey(string key)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}