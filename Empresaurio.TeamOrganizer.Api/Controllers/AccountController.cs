using Empresaurio.TeamOrganizer.Api.Responses;
using Empresaurio.TeamOrganizer.Domain.CustomEntities;
using Empresaurio.TeamOrganizer.Domain.DTOs;
using Empresaurio.TeamOrganizer.Domain.Entities;
using Empresaurio.TeamOrganizer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _AccountRepository;
        private readonly IConfiguration _Configuration;

        public AccountController(IConfiguration configuration, IAccountRepository accountRepository)
        {
            _AccountRepository = accountRepository;
            _Configuration = configuration;
        }

        [HttpPost("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromForm]Credentials credentials) 
        {
            var User = await _AccountRepository.AuthenticateAsync(credentials);

            if (User == null) 
            {
                return NoContent();
            }

            var UserCredentials = CreateJWT(User);

            CredentialsResponse response = new CredentialsResponse() 
            {
                EmailAddress = UserCredentials.EmailAddress,
                Token = UserCredentials.Token
            };

            return Ok(response);
        }

        [HttpPost("CreateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromForm] UserDTO userDTO)
        {
            User user = new User()
            {
                EmailAddress = userDTO.EmailAddress,
                Password = userDTO.Password,
                PhotoUrl = userDTO.PhotoUrl,
                FirstName = userDTO.FirstName,
                MiddleName = userDTO.MiddleName,
                LastName = userDTO.LastName,
                DateOfBirth = userDTO.DateOfBirth,
                Telephone = userDTO.Telephone
            };

            await _AccountRepository.RegisterAsync(user);

            userDTO = new UserDTO()
            {
                EmailAddress = user.EmailAddress,
                Password = user.Password,
                PhotoUrl = user.PhotoUrl,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Telephone = user.Telephone
            };

            return Ok(userDTO);
        }

        private UserCredentials CreateJWT(UserCredentials user)
        {
            var _SymmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_Configuration["JWT:SigningKey"]));

            var _SigningCredentials = new SigningCredentials(
                _SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var _Header = new JwtHeader(_SigningCredentials);

            var _Claims = new List<Claim>()
            {
                new Claim("email", user.EmailAddress),
                new Claim("name", user.FirstName),
                new Claim("lastname", user.LastName)
            };

            var _Payload = new JwtPayload(
                issuer: _Configuration["JWT:Issuer"],
                audience: _Configuration["JWT:Audience"],
                claims: _Claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(30));

            var _Token = new JwtSecurityToken(_Header, _Payload);

            user.Token = new JwtSecurityTokenHandler().WriteToken(_Token);

            return user;
        }
    }
}
