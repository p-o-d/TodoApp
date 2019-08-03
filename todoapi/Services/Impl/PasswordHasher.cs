using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using todoapi.AppOptions;

namespace todoapi.Services.Impl
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int _saltSize = 16;
        private const int _keySize = 32;

        private readonly HashOptions _options;

        public PasswordHasher(IOptions<HashOptions> hashOptions)
        {
            _options = hashOptions.Value;
        }
        public (bool verified, bool needUpdate) Check(string hash, string password)
        {
            throw new System.NotImplementedException();
        }

        public string Hash(string password)
        {
            if(string.IsNullOrEmpty(password))
                throw new ArgumentNullException();

            using (var hashFunc = new Rfc2898DeriveBytes(
                password, 
                _saltSize, 
                _options.HashIterations, 
                HashAlgorithmName.SHA256))
            {
                var salt = Convert.ToBase64String(hashFunc.Salt);
                var key = Convert.ToBase64String(hashFunc.GetBytes(_keySize));

                return $"{_options.HashIterations}.{salt}.{key}";
            }
        }
    }
}