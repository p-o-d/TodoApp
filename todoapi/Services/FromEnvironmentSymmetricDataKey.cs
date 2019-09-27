using System;

namespace todoapi.Services
{
    public class FromEnvironmentSymmetricDataKey : SymmetricDataKey
    {
        private const string ENV_VAR_DATA_KEY = "ASPNETCORE_TODOAPI_DATA_KEY";

        public FromEnvironmentSymmetricDataKey() : base(Environment.GetEnvironmentVariable(ENV_VAR_DATA_KEY))
        {
        }
    }
}