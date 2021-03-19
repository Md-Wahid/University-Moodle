using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        // public AccountController(DataContext context)
        // {
        //     _context = context;
        // }
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.email)) return BadRequest("Already Registered");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = "",
                Email = registerDto.email.ToLower(),
                AboutUser = "",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
        // public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        // {
        //     if (await UserExists(registerDto.email)) return BadRequest("Already Registered");

        //     using var hmac = new HMACSHA512();

        //     var user = new AppUser
        //     {
        //         UserName = "",
        //         Email = registerDto.email.ToLower(),
        //         AboutUser = "",
        //         PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
        //         PasswordSalt = hmac.Key
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();
        //     return user;
        // }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == loginDto.email);

            if (user == null) return Unauthorized("Invalid Email");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
        // public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        // {
        //     var user = await _context.Users
        //         .SingleOrDefaultAsync(x => x.Email == loginDto.email);

        //     if (user == null) return Unauthorized("Invalid Email");

        //     using var hmac = new HMACSHA512(user.PasswordSalt);

        //     var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

        //     for (int i = 0; i < computedHash.Length; i++)
        //     {
        //         if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        //     }

        //     return user;
        // }

        private async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}