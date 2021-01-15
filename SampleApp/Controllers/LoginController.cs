//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SampleApp.Repository;
//using System.Data;
//using SampleApp.Models;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using System.Security.Claims;
//using System.IdentityModel.Tokens.Jwt;

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ILoginRepository _iLoginRepo;
        //private IDbConnection _dbConnection;
        public LoginController(IConfiguration configuration, ILogger<LoginController> logger, ILoginRepository iLoginRepo)
        {
            _configuration = configuration;
            _logger = logger;
            _iLoginRepo = iLoginRepo;
            //_dbConnection = dbConnection;
            //objLoginRepo = new LoginRepository(_dbConnection);
        }
        //[HttpPost("ValidateUser")]
        //public IActionResult Post(Login objLogin)
        //{
        //    _logger.LogInformation("In : LoginController - ValidateUser.");
        //    bool bUserExists = objLoginRepo.ValidateUser(objLogin);
        //    if (bUserExists)
        //        return this.StatusCode(StatusCodes.Status200OK, bUserExists);
        //    else return this.StatusCode(StatusCodes.Status404NotFound, bUserExists);
        //}

        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken(Login objLogin)
        {
            IActionResult response = Unauthorized();
            var objUser = _iLoginRepo.GetUser(objLogin);
            if (objUser == null)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized user.");
            }
            string key = _configuration["Jwt:Key"];
            string issuer = _configuration["Jwt:Issuer"];
            string strToken = _iLoginRepo.CreateToken(objUser, key, issuer);
            response = Ok(new { token = strToken });
            return response;
        }

        //[HttpGet("Login")]
        //public IActionResult Post(string token)
        //{
        //    if (!objLoginRepo.ValidateCurrentToken(token))
        //        return this.StatusCode(StatusCodes.Status401Unauthorized, "Unauthoruzed.");
        //    var stream = token;
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(stream);
        //    var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
        //    var jti = tokenS.Claims.First(claim => claim.Type == "jti").Value;
        //    return this.StatusCode(StatusCodes.Status200OK, "");
        //}
    }
}