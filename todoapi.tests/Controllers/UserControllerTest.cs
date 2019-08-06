using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using todoapi.Controllers;
using todoapi.Dtos;
using todoapi.Entities;
using todoapi.Services;
using Xunit;

namespace todoapi.tests.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public void Should_Login_Registered_User()
        {
        //Given
            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(m => m.Login(It.IsNotNull<AuthCredentialsDto>()))
                .Returns(new User
                { 
                    Id = 1, 
                    Name = "testname", 
                    Email = "test@test.com", 
                    Hash = @"20.4gbglLwieS4BxkH/OvW+gw==.B+HHKuYUkj9mXmhyVHsfDxlsQXGpRdceL+5wy3HK3/k="
                });

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<AuthUserDto>(It.IsAny<User>()))
                .Returns(new AuthUserDto
                {
                    Id = 1,
                    Name = "testname",
                });

            string signatureKeyString = "CxUZLVUtuT2Yi42H9WnBkdcLfLkucZcj";
            var signatureKeyMock = new Mock<ISignatureEncodingKey>();
            signatureKeyMock
                .Setup(m => m.Algorithm)
                .Returns(SecurityAlgorithms.HmacSha256);
            signatureKeyMock
                .Setup(m => m.Key)
                .Returns(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKeyString)));

            string dataKeyString = "xPriGLcM8a5AraSioxAVdIYnIU92dpACWUVZxo15M5CJg5aH5pIon9EHqXnOASsA";
            var dataKeyMock = new Mock<IDataEncodingKey>();
            dataKeyMock
                .Setup(m => m.EncryptingAlgorithm)
                .Returns(SecurityAlgorithms.Aes256CbcHmacSha512);
            dataKeyMock
                .Setup(m => m.SigningAlgorithm)
                .Returns(JwtConstants.DirectKeyUseAlg);
            dataKeyMock
                .Setup(m => m.Key)
                .Returns(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dataKeyString)));

            var credentials = new AuthCredentialsDto{Email = "test@test.com", Password = "testpass"};
            var userController = new UserController(userServiceMock.Object, mapperMock.Object);
        //When
            var loginResult = userController.Login(credentials, 
                                                   signatureKeyMock.Object, 
                                                   dataKeyMock.Object);
        //Then
            Assert.IsAssignableFrom<OkObjectResult>(loginResult);
        }
    }
}