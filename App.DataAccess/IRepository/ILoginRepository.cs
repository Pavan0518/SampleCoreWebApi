using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Repository
{
    public interface ILoginRepository
    {
        //int Create(MdlUsers objUser);
        //bool ValidateUser(Login objLogin);
        LoginResponse GetUser(Login objLogin);
        string CreateToken(LoginResponse objLogin, string key, string issuer);
        bool ValidateCurrentToken(string token, string key, string issuer);
    }
}
