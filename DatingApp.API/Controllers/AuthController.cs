
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repository, IConfiguration config)
        {
            _config = config;
            _repo = repository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {

            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

            if (await _repo.UserExists(userForRegisterDto.UserName))
                return BadRequest("User already exists");

            var userToCreate = new User
            {
                UserName = userForRegisterDto.UserName
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);


        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login(UserForLoginto userForLoginto)
        {   

               // throw new Exception("no no ..!!");


            var userFromRepo = await _repo.Login(userForLoginto.Username.ToLower(), userForLoginto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier , userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName.ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key , SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                   Subject = new ClaimsIdentity(claims), 
                   Expires = DateTime.Now.AddDays(1),
                   SigningCredentials = creds 
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {

                token = tokenHandler.WriteToken(token)
            });


        }
    }
}