using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public bool AddEmployee(EmployeeInfo employee)
        {
            var employeeEntity = _mapper.Map<Employee>(employee);
            return _employeeRepository.SaveEmployee(employeeEntity);
        }

        public bool DeleteEmployee(string employeeCode)
        {
            return _employeeRepository.DeleteEmployee(employeeCode);
        }

        public IEnumerable<EmployeeInfo> GetAllEmployees()
        {
            var res = _employeeRepository.GetAll();
            return _mapper.Map<IEnumerable<EmployeeInfo>>(res);
        }

        public EmployeeInfo GetEmployeeByCode(string employeeCode)
        {
            var result = _employeeRepository.GetEmployeeByCode(employeeCode);
            return _mapper.Map<EmployeeInfo>(result);
        }
    }
}
