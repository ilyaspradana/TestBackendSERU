using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestBackendSERU.Entity;
using TestBackendSERU.IService;
using static TestBackendSERU.Models.Response;
using static TestBackendSERU.Models.User;

namespace TestBackendSERU.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenService _jwtTokenService;

        public UserRepository(VehicleDbContext dbContext, IConfiguration configuration, JwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }

        public DtoLogin Login([FromBody] Login userLogin)
        {
            if (!IsUserExist(userLogin.Username))
                return null;

            var user = GetUser(userLogin.Username);
            if (user == null) return null;

            //var Token = GetJWTToken(userLogin.Username);
            var Token = _jwtTokenService.GenerateToken(userLogin.Username, new List<string> { user.Is_Admin ? "Admin" : "User" });

            var data = new DtoLogin()
            {
                Name = user.Name,
                Is_Admin = user.Is_Admin,
                Token = Token
            };

            return data;
        }

        public async Task<Registration> Registration(Registration data)
        {
            if (IsUserExist(data.Username))
                return null;

            var user = new User()
            {
                Name = data.Username,
                Is_Admin = false,
                Created_At = DateTime.Now.ToString(),
                Updated_At = DateTime.Now.ToString()
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public User GetUser(string user)
        {
            return _dbContext.User.FromSqlInterpolated($"SELECT * FROM dbo.[User] WHERE Name = {user}").SingleOrDefault();
        }

        public async Task<Default> Edit(EditUser data)
        {
            var r = new Default();

            var user = await GetById(data.ID);

            if (user == null)
            {
                r.Status = "Failed";
                r.Message = "User Tidak Ditemukan";

                return r;
            }

            user.Is_Admin = data.Is_Admin;

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "User berhasil diubah";

            return r;
        }

        public bool IsUserExist(string user)
        {
            var a = _dbContext.User
            .FromSqlInterpolated($"SELECT Name FROM dbo.[User] WHERE Name = {user}")
            .Select(u => new DtoCheckName { Name = u.Name })
            .ToList();

            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public async Task<User> GetById(int id)
        {
            return await _dbContext.User
                .FromSqlInterpolated($"SELECT * FROM dbo.[User] WHERE ID = {id}")
                .SingleOrDefaultAsync();
        }

        public string GetJWTToken(string username) 
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
