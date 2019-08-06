using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace todoapi.Services.Impl
{
    //TODO: Implement Asymmetric signature key
    public class SymmetricSignatureKey : ISignatureEncodingKey, ISignatureDecodingKey
    {
        public string Algorithm => SecurityAlgorithms.HmacSha256;

        public SecurityKey Key { get; }

        public SymmetricSignatureKey(string key)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}