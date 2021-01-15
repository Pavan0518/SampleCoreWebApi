using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SampleApp.Models;
using SampleApp.Repository;

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IUserSignUpRepository _iLoginRepo;
        public SignUpController(ILogger<SignUpController> logger, IUserSignUpRepository iLoginRepo)
        {
            _logger = logger;
            _iLoginRepo = iLoginRepo;
        }
        [HttpPost("CreateUser")]
        public IActionResult Post(MdlUsers objUser)
        {
            _logger.LogInformation("In : LoginController - Post.");
            int result = _iLoginRepo.Create(objUser);
            if (result == 1)
            {
                _logger.LogInformation("Out : User created successfully.");
                return this.StatusCode(StatusCodes.Status201Created, "User created successfully.");

            }
            _logger.LogError("Out :Internal Server Error.");
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.");
        }
    }
}