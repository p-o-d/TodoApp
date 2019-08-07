using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using todoapi.AppOptions;
using todoapi.Contracts;

namespace todoapi.Services
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
            if(string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException();

            var parts = hash.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if(parts.Length != 3)
                throw new FormatException("Given hash has wrong format!");

            var iterations = Int32.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using (var hashFunc = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                var keyToCheck = hashFunc.GetBytes(_keySize);

                return (keyToCheck.SequenceEqual(key), _options.HashIterations != iterations);
            }
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