using Microsoft.Extensions.Options;
using todoapi.AppOptions;

namespace todoapi.Services
{
    public class FromOptionsSymmetricSignatureKey : SymmetricSignatureKey
    {
        public FromOptionsSymmetricSignatureKey(IOptions<DevKeysOptions> options) : base(options.Value.SignatureKey)
        {
        }
    }
}