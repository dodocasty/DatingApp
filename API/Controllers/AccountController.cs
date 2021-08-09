
using API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    

    public class AccountController:BaseApiController
    {

        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        public AccountController(DataContext context,ITokenService tokenService){
          
          _context=context;
          _tokenService = tokenService;
  


        }
        [HttpPost("register")]


        public async Task<ActionResult<UserDTO>> Register(RegisterDto  registerDto){

            if(await UserExists(registerDto.UserName)) return BadRequest("Username is taken");
            using var hmac= new HMACSHA512();
            var user = new AppUser{
                  UserName=registerDto.UserName.ToLower(),
                  PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                  PasswordSalt=hmac.Key


            };
            _context.Users.Add(user);
           await  _context.SaveChangesAsync();
           return new UserDTO{
               Username=user.UserName,
               Token=_tokenService.CreateToken(user),
           };

        }
            [HttpPost("login")]
             public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO){

                 var user = await _context.Users
                 .SingleOrDefaultAsync(x=>x.UserName==loginDTO.UserName);
                 

                 if(user== null) return Unauthorized("Invalid Username");
                 using var hmac= new HMACSHA512(user.PasswordSalt);
                 var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                 for(int i=0; i < computedHash.Length; i++){

                     if(computedHash[i] != user.PasswordHash[i])return Unauthorized("Invalid password");
                     
                 }
               return new UserDTO{
               Username=user.UserName,
               Token=_tokenService.CreateToken(user),
           };
                







             }

        private async Task<bool> UserExists(string username)
        {

            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
    }
}