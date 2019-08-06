using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using todoapi.Dtos;
using todoapi.Services;

namespace todoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService authService, IMapper mapper)
        {
            _userService = authService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Login([FromBody] AuthCredentialsDto credentials,
                                   [FromServices] ISignatureEncodingKey signatureKey,
                                   [FromServices] IDataEncodingKey dataKey)
        {
            var user = _userService.Login(credentials);
            if(user == null)
                return BadRequest( new { message = "User email or password is incorrect!."});

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                issuer: "todoapp",
                audience: "todoappclient",
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                issuedAt: DateTime.Now,
                signingCredentials: new SigningCredentials(
                    signatureKey.Key,
                    signatureKey.Algorithm),
                encryptingCredentials: new EncryptingCredentials(
                    dataKey.Key,
                    dataKey.SigningAlgorithm,
                    dataKey.EncryptingAlgorithm));

            var authUser = _mapper.Map<AuthUserDto>(user);
            authUser.Token = tokenHandler.WriteToken(token);

            return Ok(authUser);
        }
    }
}