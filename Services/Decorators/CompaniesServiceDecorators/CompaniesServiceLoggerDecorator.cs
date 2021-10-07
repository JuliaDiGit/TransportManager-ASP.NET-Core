using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Models;
using Services.Abstract;

namespace Services.Decorators.CompaniesServiceDecorators
{
    public class CompaniesServiceLoggerDecorator : ICompaniesService
    {
        private readonly ILogger<CompaniesServiceLoggerDecorator> _logger;
        private readonly ICompaniesService _inner;
        private const string CompanyId = "CompanyId";

        public CompaniesServiceLoggerDecorator(ILogger<CompaniesServiceLoggerDecorator> logger, ICompaniesService inner)
        {
            _logger = logger;
            _inner = inner;
        }

        public async Task<Company> GetCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            try
            {
                Company company = await _inner.GetCompanyByCompanyIdAsync(companyId, userLogin);

                string status = company == null
                                ? Resources.Status_NotFound
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetCompanyByCompanyId} " +
                                       $"({CompanyId} {companyId}) - " +
                                       $"{status}");

                return company;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetCompanyByCompanyId} " +
                                 $"({CompanyId} {companyId}) - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Company> AddCompanyAsync(CompanyModel companyModel, string userLogin)
        {
            try
            {
                Company company = await _inner.AddCompanyAsync(companyModel, userLogin);

                string status = company == null 
                                ? Resources.Status_Fail 
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_AddCompany} " +
                                       $"({CompanyId} {companyModel.CompanyId}) - " + 
                                       $"{status}");

                return company;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_AddCompany} " +
                                 $"({CompanyId} {companyModel.CompanyId}) - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Company> UpdateCompanyAsync(CompanyModel companyModel, string userLogin)
        {
            try
            {
                Company company = await _inner.UpdateCompanyAsync(companyModel, userLogin);

                string status = company == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_UpdateCompany} " +
                                       $"({CompanyId} {companyModel.CompanyId}) - " +
                                       $"{status}");

                return company;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_UpdateCompany} " +
                                 $"({CompanyId} {companyModel.CompanyId}) - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<Company> DeleteCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            try
            {
                Company company = await _inner.DeleteCompanyByCompanyIdAsync(companyId, userLogin);

                string status = company == null 
                                ? Resources.Status_Fail 
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_DeleteCompanyByCompanyId} " +
                                       $"({CompanyId} {companyId}) - " + 
                                       $"{status}");

                return company;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_DeleteCompanyByCompanyId} " +
                                 $"({CompanyId} {companyId}) - " +
                                 $"{e.GetType()}");

                throw;
            }
        }

        public async Task<List<Company>> GetAllCompaniesAsync(string userLogin)
        {
            try
            {
                List<Company> companies = await _inner.GetAllCompaniesAsync(userLogin);

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetAllCompanies} - " +
                                       $"{Resources.Status_Success}");

                return companies;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_GetAllCompanies} - " +
                                 $"{e.GetType()}");
                throw;
            }
        }

        public async Task<Company> RemoveCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            try
            {
                Company company = await _inner.RemoveCompanyByCompanyIdAsync(companyId, userLogin);

                string status = company == null
                                ? Resources.Status_Fail
                                : Resources.Status_Success;

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_RemoveCompanyByCompanyId} " +
                                       $"({CompanyId} {companyId}) - " + 
                                       $"{status}");

                return company;
            }
            catch (Exception e)
            {
                _logger.LogError($"{userLogin} - " +
                                 $"{Resources.Operation_RemoveCompanyByCompanyId} " +
                                 $"({CompanyId} {companyId}) - " +
                                 $"{e.GetType()}");

                throw;
            }
        }
    }
}