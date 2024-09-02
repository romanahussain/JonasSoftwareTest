using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;
using NLog;

namespace WebApi.Controllers
{
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
            Logger.Info("CompanyController instantiated.");
        }

        // GET api/<controller>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            Logger.Info("GetAll method called.");
            try
            {
                var items = await Task.Run(() => _companyService.GetAllCompanies());
                var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(items);
                Logger.Info("GetAll method executed successfully.");
                return Ok(companyDtos);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while getting all companies.");
                return InternalServerError(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{companyCode}")]
        public async Task<IHttpActionResult> Get(string companyCode)
        {
            Logger.Info($"Get method called with companyCode: {companyCode}");
            try
            {
                var item = await Task.Run(() => _companyService.GetCompanyByCode(companyCode));
                if (item == null)
                {
                    Logger.Warn($"Company with code {companyCode} not found.");
                    return NotFound();
                }

                var companyDto = _mapper.Map<CompanyDto>(item);
                Logger.Info($"Get method executed successfully for companyCode: {companyCode}");
                return Ok(companyDto);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"An error occurred while getting the company with code {companyCode}.");
                return InternalServerError(ex);
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            Logger.Info("Post method called.");
            if (companyDto == null)
            {
                Logger.Warn("Received null companyDto in Post method.");
                return BadRequest("Company data is null.");
            }

            try
            {
                var existingCompany = await Task.Run(() =>
                    _companyService.GetCompanyByCode(companyDto.CompanyCode));

                if (existingCompany != null && existingCompany.SiteId == companyDto.SiteId)
                {
                    Logger.Warn($"Company with SiteId {companyDto.SiteId} and CompanyCode {companyDto.CompanyCode} already exists.");
                    return BadRequest("A company with the same SiteId and CompanyCode already exists.");
                }

                var company = _mapper.Map<CompanyInfo>(companyDto);
                bool isAdded = await Task.Run(() => _companyService.AddCompany(company));

                if (!isAdded)
                {
                    Logger.Warn("Failed to add the company.");
                    return BadRequest("Failed to add the company.");
                }

                Logger.Info("Company added successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while adding the company.");
                return InternalServerError(ex);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{companyCode}")]
        public async Task<IHttpActionResult> Put(string companyCode, [FromBody] CompanyDto companyDto)
        {
            Logger.Info($"Put method called with companyCode: {companyCode}");
            if (companyDto == null)
            {
                Logger.Warn("Received null companyDto in Put method.");
                return BadRequest("Company data is null.");
            }

            try
            {
                var existingCompany = await Task.Run(() => _companyService.GetCompanyByCode(companyCode));
                if (existingCompany == null)
                {
                    Logger.Warn($"Company with code {companyCode} not found.");
                    return NotFound();
                }

                // Check if SiteId is included in the update request
                if (!string.IsNullOrEmpty(companyDto.SiteId))
                {
                    Logger.Warn("Update request contains SiteId, which is not allowed.");
                    return BadRequest("SiteId cannot be updated.");
                }

                // Check if CompsnyCode in body is included in the update request
                if (!string.IsNullOrEmpty(companyDto.CompanyCode))
                {
                    Logger.Warn("Update request contains CompanyCode, which is not allowed.");
                    return BadRequest("CompanyCode cannot be updated.");
                }

                var company = _mapper.Map<CompanyInfo>(companyDto);
                company.CompanyCode = companyCode; // Ensure the company code stays consistent

                bool isUpdated = await Task.Run(() => _companyService.AddCompany(company)); // Assuming this method should be used for updates

                if (!isUpdated)
                {
                    Logger.Warn($"Failed to update the company with code {companyCode}.");
                    return BadRequest("Failed to update the company.");
                }

                Logger.Info($"Company with code {companyCode} updated successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"An error occurred while updating the company with code {companyCode}.");
                return InternalServerError(ex);
            }
        }


        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("{companyCode}")]
        public async Task<IHttpActionResult> Delete(string companyCode)
        {
            Logger.Info($"Delete method called with companyCode: {companyCode}");
            try
            {
                var existingCompany = await Task.Run(() => _companyService.GetCompanyByCode(companyCode));
                if (existingCompany == null)
                {
                    Logger.Warn($"Company with code {companyCode} not found.");
                    return NotFound();
                }

                bool isDeleted = await Task.Run(() => _companyService.DeleteCompany(companyCode));

                if (!isDeleted)
                {
                    Logger.Warn($"Failed to delete the company with code {companyCode}.");
                    return BadRequest("Failed to delete the company.");
                }

                Logger.Info($"Company with code {companyCode} deleted successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"An error occurred while deleting the company with code {companyCode}.");
                return InternalServerError(ex);
            }
        }
    }
}
