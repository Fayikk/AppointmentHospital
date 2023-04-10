using AppointmentHospital.Server.Context;
using AppointmentHospital.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AppointmentHospital.Server.Services.ForAuth
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(IConfiguration configuration,DataContext context, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public string GetUserEmail()
        {
            return _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public int GetUserId()
        {
            return int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Message = "User is not found";
                response.Success = false;
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Please check your password";
            }
            else
            {
                response.Success = true;
                response.Data = CreateToken(user);
                response.Message = "User Login Successfully";
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exist",
                };
            }
            CreatePasswordHash(password,out byte[] passwordHash,out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.DateCreated = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = user.Id,
                Success = true,
                Message = "User created succesfully",
            };
        }

        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash =hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }

        private async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x=>x.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Role,user.Role),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public Task<ServiceResponse<bool>> SelectAdmin(string email)
        {
            throw new NotImplementedException();
        }
    }
}
