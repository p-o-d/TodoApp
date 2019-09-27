using System.Text;
using Microsoft.IdentityModel.Tokens;
using todoapi.Contracts;

namespace todoapi.Services
{
    //TODO: Implement Asymmetric signature key
    public abstract class SymmetricSignatureKey : ISignatureEncodingKey, ISignatureDecodingKey
    {
        public string Algorithm => SecurityAlgorithms.HmacSha256;

        public SecurityKey Key { get; }

        protected SymmetricSignatureKey(string key)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}