using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public bool DeleteEmployee(string employeeCode)
        {
            return _employeeDbWrapper.Delete(e => e.EmployeeCode.Equals(employeeCode));
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeDbWrapper.FindAll();
        }

        public Employee GetEmployeeByCode(string employeeCode)
        {
            return _employeeDbWrapper.Find(e => e.EmployeeCode.Equals(employeeCode))?.FirstOrDefault();
        }

        public bool SaveEmployee(Employee employee)
        {
            // Check if the employee already exists
            var existingEmployee = _employeeDbWrapper.Find(e =>
                e.EmployeeCode.Equals(employee.EmployeeCode))?.FirstOrDefault();

            if (existingEmployee != null)
            {
                // Update existing employee details only if new values are provided
                if (!string.IsNullOrEmpty(employee.EmployeeName))
                    existingEmployee.EmployeeName = employee.EmployeeName;

                if (!string.IsNullOrEmpty(employee.OccupationName))
                    existingEmployee.OccupationName = employee.OccupationName;

                if (!string.IsNullOrEmpty(employee.EmployeeStatus))
                    existingEmployee.EmployeeStatus = employee.EmployeeStatus;

                if (!string.IsNullOrEmpty(employee.EmailAddress))
                    existingEmployee.EmailAddress = employee.EmailAddress;

                if (!string.IsNullOrEmpty(employee.PhoneNumber))
                    existingEmployee.PhoneNumber = employee.PhoneNumber;

                if (employee.LastModifiedDateTime != null)
                    existingEmployee.LastModifiedDateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (employee.SiteId != null)
                    existingEmployee.SiteId = employee.SiteId;

                if (employee.CompanyCode != null)
                    existingEmployee.CompanyCode = employee.CompanyCode;

                if (!string.IsNullOrEmpty(employee.CompanyName))
                    existingEmployee.CompanyName = employee.CompanyName;

                return _employeeDbWrapper.Update(existingEmployee);
            }


            // Insert new employee
            return _employeeDbWrapper.Insert(employee);
        }

    }
}
