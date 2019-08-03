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

        [Fact]
        public void Should_Verify_Valid_Hash_Without_NeedUpdate()
        {
        //Given
            var options = Options.Create(new HashOptions{ HashIterations = 20 });
            var hash = @"20.4gbglLwieS4BxkH/OvW+gw==.B+HHKuYUkj9mXmhyVHsfDxlsQXGpRdceL+5wy3HK3/k=";
            var password = "testpass";
            var hasher = new PasswordHasher(options);
        //When
            var checkResult = hasher.Check(hash, password);
        //Then
            Assert.True(checkResult.verified);
            Assert.False(checkResult.needUpdate);
        }

        [Fact]
        public void Should_Return_NeedUpdate_On_Mismatch_Iterations()
        {
        //Given
            var options = Options.Create(new HashOptions{ HashIterations = 50 });
            var hash = @"20.4gbglLwieS4BxkH/OvW+gw==.B+HHKuYUkj9mXmhyVHsfDxlsQXGpRdceL+5wy3HK3/k=";
            var password = "testpass";
            var hasher = new PasswordHasher(options);
        //When
            var checkResult = hasher.Check(hash, password);
        //Then
            Assert.True(checkResult.needUpdate);
        }

        [Fact]
        public void Should_Not_Verify_Invalid_Hash()
        {
        //Given
            var options = Options.Create(new HashOptions{ HashIterations = 20 });
            var hash = @"20.4gbglLwieS4BxkH/OvW+gw==.B+HHKuYUkj9mXmhyVHsfDxlsQXGpRdceL+5wy3HK3/k=";
            var password = "invalidpass";
            var hasher = new PasswordHasher(options);
        //When
            var checkResult = hasher.Check(hash, password);
        //Then
            Assert.False(checkResult.verified);
        }

        [Fact]
        public void Should_Throw_On_Invalid_Hash_Format()
        {
        //Given
            var options = Options.Create(new HashOptions{ HashIterations = 20 });
            var hash = "invalidhash";
            var password = "invalidpass";
            var hasher = new PasswordHasher(options);
        //Then
            Assert.Throws<FormatException>(() => hasher.Check(hash, password));
        }
    }
}