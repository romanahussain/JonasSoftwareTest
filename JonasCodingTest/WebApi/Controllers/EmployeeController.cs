using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;
using NLog;
using System.Linq;

namespace WebApi.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET api/employee
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            Logger.Info("Fetching all employees.");
            try
            {
                var items = await Task.Run(() => _employeeService.GetAllEmployees());
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(items);
                Logger.Info("Fetched {Count} employees.", employeeDtos?.Count() ?? 0);
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while fetching all employees.");
                return InternalServerError(ex);
            }
        }

        // GET api/employee/{employeeCode}
        [HttpGet]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> Get(string employeeCode)
        {
            Logger.Info("Fetching employee with code: {EmployeeCode}", employeeCode);
            try
            {
                var item = await Task.Run(() => _employeeService.GetEmployeeByCode(employeeCode));
                if (item == null)
                {
                    Logger.Warn("Employee with code {EmployeeCode} not found.", employeeCode);
                    return NotFound();
                }

                var employeeDto = _mapper.Map<EmployeeDto>(item);
                Logger.Info("Fetched employee with code: {EmployeeCode}", employeeCode);
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while fetching employee with code: {EmployeeCode}", employeeCode);
                return InternalServerError(ex);
            }
        }

        // POST api/employee
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                Logger.Warn("Received null employee data in POST request.");
                return BadRequest("Employee data is null.");
            }

            try
            {
                // Check if an employee with the same EmployeeCode already exists
                var existingEmployee = await Task.Run(() => _employeeService.GetEmployeeByCode(employeeDto.EmployeeCode));
                if (existingEmployee != null)
                {
                    Logger.Warn("Employee with code {EmployeeCode} already exists.", employeeDto.EmployeeCode);
                    return BadRequest("An employee with the same EmployeeCode already exists.");
                }

                Logger.Info("Adding new employee: {EmployeeName}", employeeDto.EmployeeName);
                var employee = _mapper.Map<EmployeeInfo>(employeeDto);
                bool isAdded = await Task.Run(() => _employeeService.AddEmployee(employee));

                if (!isAdded)
                {
                    Logger.Error("Failed to add the employee: {EmployeeName}", employeeDto.EmployeeName);
                    return BadRequest("Failed to add the employee.");
                }

                Logger.Info("Successfully added employee: {EmployeeName}", employeeDto.EmployeeName);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while adding employee: {EmployeeName}", employeeDto.EmployeeName);
                return InternalServerError(ex);
            }
        }

        // PUT api/employee/{employeeCode}
        [HttpPut]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> Put(string employeeCode, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                Logger.Warn("Received null employee data in PUT request for employeeCode: {EmployeeCode}", employeeCode);
                return BadRequest("Employee data is null.");
            }

            try
            {
                Logger.Info("Updating employee with code: {EmployeeCode}", employeeCode);
                var existingEmployee = await Task.Run(() => _employeeService.GetEmployeeByCode(employeeCode));
                if (existingEmployee == null)
                {
                    Logger.Warn("Employee with code {EmployeeCode} not found for update.", employeeCode);
                    return NotFound();
                }

                // Check if EmployeeCode is included in the update request
                if (!String.IsNullOrEmpty(employeeDto.EmployeeCode))
                {
                    Logger.Warn("Update request contains  Employee Code, which is not allowed.");
                    return BadRequest("EmployeeCode cannot be updated.");
                }

                var employee = _mapper.Map<EmployeeInfo>(employeeDto);
                employee.EmployeeCode = employeeCode; // Ensure the employee code stays consistent

                bool isUpdated = await Task.Run(() => _employeeService.AddEmployee(employee)); // Assuming UpdateEmployee method exists

                if (!isUpdated)
                {
                    Logger.Error("Failed to update the employee with code: {EmployeeCode}", employeeCode);
                    return BadRequest("Failed to update the employee.");
                }

                Logger.Info("Successfully updated employee with code: {EmployeeCode}", employeeCode);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while updating employee with code: {EmployeeCode}", employeeCode);
                return InternalServerError(ex);
            }
        }

        // DELETE api/employee/{employeeCode}
        [HttpDelete]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> Delete(string employeeCode)
        {
            Logger.Info("Deleting employee with code: {EmployeeCode}", employeeCode);
            try
            {
                var existingEmployee = await Task.Run(() => _employeeService.GetEmployeeByCode(employeeCode));
                if (existingEmployee == null)
                {
                    Logger.Warn("Employee with code {EmployeeCode} not found for deletion.", employeeCode);
                    return NotFound();
                }

                bool isDeleted = await Task.Run(() => _employeeService.DeleteEmployee(employeeCode));

                if (!isDeleted)
                {
                    Logger.Error("Failed to delete the employee with code: {EmployeeCode}", employeeCode);
                    return BadRequest("Failed to delete the employee.");
                }

                Logger.Info("Successfully deleted employee with code: {EmployeeCode}", employeeCode);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while deleting employee with code: {EmployeeCode}", employeeCode);
                return InternalServerError(ex);
            }
        }
    }
}
