using System;
using System.Collections.Generic;
using System.Data;
//using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
//using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SampleApp.Filters.Exception;
using SampleApp.Models;
using SampleApp.Repository;

namespace SampleApp.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeRepository _iEmpRepo;
        public EmployeeController(IConfiguration configuration, ILogger<EmployeeController> logger, IEmployeeRepository iEmpRepo)
        {
            _logger = logger;
            _iEmpRepo = iEmpRepo;
        }
        // GET api/values
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> Get()
        {
            //int[] arr = { 1, 2, 3 };
            //int b = arr[10];
            _logger.LogInformation("In Get All Employees.");
            var objEmps = await _iEmpRepo.GetAllEmployees();
            if (objEmps != null)
            {
                _logger.LogInformation("Out Get All Employees. Success.");
                return this.StatusCode(StatusCodes.Status200OK, objEmps);

            }
            else
            {
                _logger.LogError("Out Get All Employees. Server Error.");
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }

        }

        [HttpGet("GetEmployeeById")]
        public async Task<IActionResult> Get(int Id)
        {
            _logger.LogInformation("In Get Employee by Id.");
            var objEmps = await _iEmpRepo.FindEmployee(Id);
            if (objEmps != null && objEmps.Count() > 0)
            {
                _logger.LogInformation("Out et Employee by Id. Success.");
                return this.StatusCode(StatusCodes.Status200OK, objEmps);
            }
            else
            {
                _logger.LogError("Out Get All Employees. Employee Not Found.");
                return this.StatusCode(StatusCodes.Status404NotFound, "Employee Not Found.");
            }
        }
        [HttpPost("SaveEmployee")]
        public async Task<IActionResult> SaveEmployee(MdlSaveEmp objEmp)
        {
            _logger.LogInformation("In Save Employee.");
            int result = await Task.Run(() => _iEmpRepo.SaveEmployee(objEmp));
            if (result == 1)
                return this.StatusCode(StatusCodes.Status201Created, "Successfully Saved");
            else
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.");
        }
        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> SaveEmployee(int Id)
        {
            int result = await Task.Run(() => _iEmpRepo.DeleteEmployee(Id));
            if (result == 1)
                return this.StatusCode(StatusCodes.Status204NoContent, "Successfully Deleted");
            else
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.");
        }
        //[HttpPost("IsEmpExists")]
        //public IActionResult IsEmpExists(int Id)
        //{
        //    int result = objEmpRepo.IsEmpExists(Id);
        //    if (result == 1)
        //        return this.StatusCode(StatusCodes.Status200OK, "Existed.");
        //    else
        //        return this.StatusCode(StatusCodes.Status404NotFound, "Not Existed.");
        //}
        [HttpPost("SearchEmployees")]
        public async Task<IActionResult> SearchEmployees(MdlSearchEmp objEmp)
        {
            var result = await _iEmpRepo.SearchEmployee(objEmp);
            if (result != null && result.Count() > 0)
                return this.StatusCode(StatusCodes.Status200OK, result);
            else
                return this.StatusCode(StatusCodes.Status404NotFound, "Not Found.");
        }
    }
}