using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetEmployeeByCode(string employeeCode);
        bool SaveEmployee(Employee employee);
        bool DeleteEmployee(string employeeCode);
    }
}
