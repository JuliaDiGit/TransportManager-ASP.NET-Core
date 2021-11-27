using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TransportManager.Domain;
using TransportManager.Models;
using TransportManager.Services.Abstract;

namespace TransportManager.Services.Decorators.CompaniesServiceDecorators
{
    public class CompaniesServiceLoggerDecorator : ICompaniesService
    {
        private readonly ILogger<CompaniesServiceLoggerDecorator> _logger;
        private readonly ICompaniesService _inner;
        private const string CompanyId = "CompanyId";

        public CompaniesServiceLoggerDecorator(ILogger<CompaniesServiceLoggerDecorator> logger, ICompaniesService inner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public async Task<Company> GetCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            try
            {
                if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var company = await _inner.GetCompanyByCompanyIdAsync(companyId, userLogin);

                var status = company == null
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
                var log = userLogin == null
                          ? $"{Resources.Operation_GetCompanyByCompanyId} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_GetCompanyByCompanyId} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Company> AddCompanyAsync(CompanyModel companyModel, string userLogin)
        {
            try
            {
                if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var company = await _inner.AddCompanyAsync(companyModel, userLogin);

                var log = company == null
                          ? $"{userLogin} - {Resources.Operation_AddCompany} - {Resources.Status_Fail}"
                          : $"{userLogin} - " +
                            $"{Resources.Operation_AddCompany} ({CompanyId} {company.CompanyId}) - " +
                            $"{Resources.Status_Success}";

                _logger.LogInformation(log);

                return company;
            }
            catch (Exception e)
            {
                var log = userLogin == null
                          ? $"{Resources.Operation_AddCompany} - {e.GetType()}"
                          : $"{userLogin} - {Resources.Operation_AddCompany} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Company> UpdateCompanyAsync(CompanyModel companyModel, string userLogin)
        {
            try
            {
                if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var company = await _inner.UpdateCompanyAsync(companyModel, userLogin);

                var status = company == null
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_UpdateCompany} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_UpdateCompany} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Company> DeleteCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            try
            {
                if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var company = await _inner.DeleteCompanyByCompanyIdAsync(companyId, userLogin);

                var status = company == null 
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_DeleteCompanyByCompanyId} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_DeleteCompanyByCompanyId} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<List<Company>> GetAllCompaniesAsync(string userLogin)
        {
            try
            {
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var companies = await _inner.GetAllCompaniesAsync(userLogin);

                _logger.LogInformation($"{userLogin} - " +
                                       $"{Resources.Operation_GetAllCompanies} - " +
                                       $"{Resources.Status_Success}");

                return companies;
            }
            catch (Exception e)
            {
                var log = userLogin == null 
                          ? $"{Resources.Operation_GetAllCompanies} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_GetAllCompanies} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }

        public async Task<Company> RemoveCompanyByCompanyIdAsync(int companyId, string userLogin)
        {
            try
            {
                if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));
                if (userLogin == null) throw new ArgumentNullException(nameof(userLogin));

                var company = await _inner.RemoveCompanyByCompanyIdAsync(companyId, userLogin);

                var status = company == null
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
                var log = userLogin == null 
                          ? $"{Resources.Operation_RemoveCompanyByCompanyId} - {e.GetType()}" 
                          : $"{userLogin} - {Resources.Operation_RemoveCompanyByCompanyId} - {e.GetType()}";

                _logger.LogError(log);

                throw;
            }
        }
    }
}