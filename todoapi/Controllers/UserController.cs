using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using todoapi.Dtos;
using todoapi.Contracts;
using Microsoft.Extensions.Options;
using todoapi.AppOptions;
using Microsoft.AspNetCore.Authorization;

namespace todoapi.Controllers
{
    [Route("api/users")]
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

        [AllowAnonymous]
        [HttpPost("value")]
        public IActionResult Value()
        {
            return Ok(5);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthCredentialsDto credentials,
                                   [FromServices] ISignatureEncodingKey signatureKey,
                                   [FromServices] IDataEncodingKey dataKey,
                                   [FromServices] IOptions<JWTOptions> jwtOpions)
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
                issuer: jwtOpions.Value.Issuer,
                audience: jwtOpions.Value.Audience,
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(jwtOpions.Value.ExpiresInMins),
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