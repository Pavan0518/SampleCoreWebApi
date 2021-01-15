using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<MdlEmpResponse>> FindEmployee(int Id);
        Task<IEnumerable<MdlEmpResponse>> GetAllEmployees();
        int SaveEmployee(MdlSaveEmp objEmp);
        int DeleteEmployee(int Id);
        Task<IEnumerable<MdlSearchEmp>> SearchEmployee(MdlSearchEmp objEmp);
        int IsEmpExists(int Id);
    }
}
