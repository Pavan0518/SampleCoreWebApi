using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using SampleApp.Models;
using System.Data;

namespace SampleApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection _dbConnection;
        public EmployeeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<MdlEmpResponse>> GetAllEmployees()
        {
            return await _dbConnection.QueryAsync<MdlEmpResponse>("Select * from Employee order by 2 desc;");
        }

        public async Task<IEnumerable<MdlEmpResponse>> FindEmployee(int Id)
        {
            return await _dbConnection.QueryAsync<MdlEmpResponse>("Select * from Employee where Id = " + Id + ";");
        }


        public int SaveEmployee(MdlSaveEmp objEmp)
        {
            string strQuery = "";
            if (objEmp.Id == 0)
                strQuery = @"Insert into Employee(FName, LName, Designation, Email, Gender) values(@FName,@LName,@Designation,@Email,@Gender)";
            else if (objEmp.Id > 0)
                strQuery = @"update Employee set FName = @FName, LName = @LName, Designation = @Designation, Email = @Email, Gender = @Gender where Id = @Id";
            return _dbConnection.Execute(strQuery, objEmp);
        }

        public int DeleteEmployee(int Id)
        {
            string strQuery = "Delete from Employee where Id = @Id";
            return _dbConnection.Execute(strQuery, new { Id = Id });
        }

        public async Task<IEnumerable<MdlSearchEmp>> SearchEmployee(MdlSearchEmp objEmp)
        {
            string strQuery = "select * from Employee";
            string strFilter = "";
            if (objEmp.Id > 0)
            {
                strFilter += string.IsNullOrEmpty(strFilter) ? "Id = @Id " : "and Id = @Id ";
            }
            if (!string.IsNullOrEmpty(objEmp.FName))
            {
                strFilter += string.IsNullOrEmpty(strFilter) ? "FName like @FName " : "and FName like @FName ";
            }
            if (!string.IsNullOrEmpty(objEmp.LName))
            {
                strFilter += string.IsNullOrEmpty(strFilter) ? "LName like @LName " : "and LName like @LName ";
            }
            if (!string.IsNullOrEmpty(objEmp.Designation))
            {
                strFilter += string.IsNullOrEmpty(strFilter) ? "Designation like @Designation " : "and Designation like @Designation ";
            }
            if (!string.IsNullOrEmpty(objEmp.Email))
            {
                strFilter += string.IsNullOrEmpty(strFilter) ? "Email like @Email " : "and Email like @Email ";
            }
            if (!string.IsNullOrEmpty(objEmp.Gender))
            {
                strFilter += string.IsNullOrEmpty(strFilter) ? "Gender like @Gender " : "and Gender like @Gender ";
            }
            strFilter = !string.IsNullOrEmpty(strFilter) ? " Where " + strFilter : "";
            strQuery = strQuery + strFilter + " order by 2 desc";
            return await Task.Run(() => _dbConnection.Query<MdlSearchEmp>(strQuery, new
            {
                FName = "%" + objEmp.FName + "%",
                LName = "%" + objEmp.LName + "%",
                Designation = "%" + objEmp.Designation + "%",
                Email = "%" + objEmp.Email + "%",
                Gender = "%" + objEmp.Gender + "%"
            }).ToList());

        }
        public int IsEmpExists(int Id)
        {
            string strQuery = "select count(*) from Employee where Id = @Id order by 2 desc";
            return _dbConnection.Query<MdlSearchEmp>(strQuery, new { Id = Id }).Count();
        }
    }
}
