using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using SampleApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Data;

namespace SampleApp.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private IDbConnection _dbConnection;
        public LoginRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        //public bool ValidateUser(Login objLogin)
        //{
        //    bool bresult = false;
        //    using (var connection = new NpgsqlConnection(_connectionString)
        //    {
        //        string strQuery = @"select * from users where email = @email and password = @password;";
        //        int result = connection.Execute(strQuery, objLogin);
        //        if (result > 0) bresult = true;
        //    };
        //    return bresult;
        //}
        public LoginResponse GetUser(Login objLogin)
        {
            LoginResponse objResponse = new LoginResponse();
            string strQuery = @"select * from users where email = '" + objLogin.email + "'";
            var objUsers = _dbConnection.Query(strQuery).FirstOrDefault();
            objResponse.id = objUsers.id;
            //objResponse.user_id = objUsers.user_id;
            objResponse.first_name = objUsers.first_name;
            objResponse.last_name = objUsers.last_name;
            objResponse.email = objUsers.email;
            objResponse.phone = objUsers.phone;
            objResponse.isActive = objUsers.isactive;
            bool bIsValid = BCrypt.Net.BCrypt.Verify(objLogin.password, objUsers.password);
            if (!bIsValid) objResponse = null;
            return objResponse;
        }

        public string CreateToken(LoginResponse objLogin, string key, string issuer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, objLogin.email),
                new Claim("DateOfJoing", DateTime.Now.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserLoginId", objLogin.email),
                new Claim("UID", objLogin.email.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;

        }
        public bool ValidateCurrentToken(string token, string key, string issuer)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
