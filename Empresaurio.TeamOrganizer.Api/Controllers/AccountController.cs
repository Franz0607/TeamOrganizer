using Empresaurio.TeamOrganizer.Domain.CustomEntities;
using Empresaurio.TeamOrganizer.Domain.DTOs;
using Empresaurio.TeamOrganizer.Domain.Entities;
using Empresaurio.TeamOrganizer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _AccountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _AccountRepository = accountRepository;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromForm]Credentials credentials) 
        {
            var User = await _AccountRepository.AuthenticateAsync(credentials);

            if (User == null) 
            {
                return NoContent();
            }

            return Ok(User);
        }

        [HttpPost("CreateAccount")]
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
    }
}
