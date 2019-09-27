using System;

namespace todoapi.Services
{
    public class FromEnvironmentSymmetricSignatureKey : SymmetricSignatureKey
    {
        private const string ENV_VAR_SIGNATURE_KEY = "ASPNETCORE_TODOAPI_SIGNATURE_KEY";

        public FromEnvironmentSymmetricSignatureKey() : base(Environment.GetEnvironmentVariable(ENV_VAR_SIGNATURE_KEY))
        {
        }
    }
}