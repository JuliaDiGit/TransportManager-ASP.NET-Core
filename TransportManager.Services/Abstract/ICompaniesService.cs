using System.Collections.Generic;
using System.Threading.Tasks;
using TransportManager.Domain;
using TransportManager.Models;

namespace TransportManager.Services.Abstract
{
    public interface ICompaniesService
    {
        Task<Company> GetCompanyByCompanyIdAsync(int companyId, string userLogin);
        Task<Company> AddCompanyAsync(CompanyModel companyModel, string userLogin);
        Task<Company> UpdateCompanyAsync(CompanyModel companyModel, string userLogin);
        Task<Company> DeleteCompanyByCompanyIdAsync(int companyId, string userLogin);
        Task<List<Company>> GetAllCompaniesAsync(string userLogin);
        Task<Company> RemoveCompanyByCompanyIdAsync(int companyId, string userLogin);
    }
}