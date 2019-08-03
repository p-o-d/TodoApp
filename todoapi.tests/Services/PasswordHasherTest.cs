using todoapi.AppOptions;
using Microsoft.Extensions.Options;
using Xunit;
using todoapi.Services.Impl;
using System;

namespace todoapi.tests.Services
{
    public class PasswordHasherTest
    {
        [Fact]
        public void Should_Create_Hash()
        {
        //Given
            var options = Options.Create(new HashOptions{ HashIterations = 20 });
            var hashInput = "testpass";
            var hasher = new PasswordHasher(options);
        //When
            var hash = hasher.Hash(hashInput);
        //Then
            Assert.False(string.IsNullOrEmpty(hash));
            var parts = hash.Split('.', StringSplitOptions.RemoveEmptyEntries);
            Assert.Collection(parts, 
                iters => Assert.Equal(Int32.Parse(iters), options.Value.HashIterations),
                salt => Assert.False(string.IsNullOrEmpty(salt)),
                password => {
                    Assert.False(string.IsNullOrEmpty(password));
                    Assert.NotEqual(password, hashInput);
                } );
        }

        [Fact]
        public void Should_Throw_On_Empty_Input()
        {
        //Given
            var options = Options.Create(new HashOptions());
            var hashInput = "";
            var hasher = new PasswordHasher(options);
        //When
            Assert.Throws<ArgumentNullException>(() => hasher.Hash(hashInput));
        }
    }
}