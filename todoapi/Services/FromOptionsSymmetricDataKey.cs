using Microsoft.Extensions.Options;
using todoapi.AppOptions;

namespace todoapi.Services
{
    public class FromOptionsSymmetricDataKey : SymmetricDataKey
    {
        public FromOptionsSymmetricDataKey(IOptions<DevKeysOptions> options) : base(options.Value.DataKey)
        {
        }
    }
}