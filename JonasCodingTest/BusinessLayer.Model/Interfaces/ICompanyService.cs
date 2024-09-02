using System.Collections.Generic;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<CompanyInfo> GetAllCompanies();
        CompanyInfo GetCompanyByCode(string companyCode);

        bool AddCompany(CompanyInfo company);

        bool DeleteCompany(string companyCode);

    }
}
