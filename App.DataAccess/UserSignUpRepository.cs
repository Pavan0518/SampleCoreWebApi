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
    public class UserSignUpRepository : IUserSignUpRepository
    {
        private IDbConnection _dbConnection;
        public UserSignUpRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public int Create(MdlUsers objUser)
        {
            int result = 0;
            string strQuery = "";
            objUser.password = BCrypt.Net.BCrypt.HashPassword(objUser.password);
            objUser.isActive = true;
            objUser.id = 0;
            objUser.user_id = "";
            objUser.phone = "";
            strQuery = @"Insert into Users(first_name, last_name, email, phone, password, isActive) values(@first_name, @last_name, @email, @phone, @password, @isActive)";
            result = _dbConnection.Execute(strQuery, objUser);
            return result;
        }
    }
}
