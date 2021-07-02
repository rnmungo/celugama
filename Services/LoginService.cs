using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CeluGamaSystem.Models;
using CeluGamaSystem.Dtos;

namespace CeluGamaSystem.Services
{
    public class LoginService : ILoginService
    {
        private readonly CeluGamaDbContext _context;
        private IConfiguration _config;

        public LoginService(CeluGamaDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Intenta loguearse con el modelo User especificado. 
        /// Si las credenciales son válidas, retorna el modelo User.
        /// Si no lo son, retorna un modelo null.
        /// </summary>
        /// <param name="user">El usuario que intentará loguearse</param>
        /// <returns></returns>
        public User tryLogin(User user)
        {
            User userFound = null;
            string encryptedPassword = Security.Security.stringToSHA256(user.Password);
            userFound = _context.Users.Where(o => o.Username == user.Username && o.Password == encryptedPassword).FirstOrDefault();

            return userFound;
        }

        /// <summary>
        /// Genera un token JWT para el username especificado.
        /// </summary>
        /// <param name="username">El nombre de usuario para quien se generará el token</param>
        /// <returns>Un token JWT</returns>
        public JWToken generateToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime expireDate = DateTime.UtcNow.AddHours(9);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: expireDate,
              signingCredentials: credentials);

            return new JWToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expireDate,
                Username = username
            };
        }

        public void changePassword(User user)
        {
            User userFound = _context.Users.Where(o => o.Username == user.Username).FirstOrDefault();
            string encryptedPassword = Security.Security.stringToSHA256(user.Password);
            userFound.Password = encryptedPassword;
            _context.Entry(userFound).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
