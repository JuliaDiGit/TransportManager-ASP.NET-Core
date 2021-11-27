using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportManager.Common.Exceptions;
using TransportManager.Models;
using TransportManager.Services.Abstract;

namespace TransportManager.Controllers
{
    /// <summary>
    ///     Контроллер компаний
    /// </summary>
    [ApiController]
    [Route("companies")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesService _companiesService;
        private readonly IMapper _mapper;

        public CompaniesController(ICompaniesService companiesService, IMapper mapper)
        {
            _companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        ///     Получить компанию
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetCompanyByCompanyIdAsync(int companyId)
        {
            if (companyId <= 0) throw new UserErrorException(Resources.Error_IncorrectCompanyId);

            var userLogin = HttpContext.User.Identity.Name;

            var company = await _companiesService.GetCompanyByCompanyIdAsync(companyId, userLogin);

            if (company == null || company.IsDeleted) return NoContent();

            var companyModel = _mapper.Map<CompanyModel>(company);

            return Ok(companyModel);
        }

        /// <summary>
        ///     Добавить компанию
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCompanyAsync(CompanyModel companyModel)
        {
            if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));
            if (companyModel.Id != 0) throw new UserErrorException(Resources.Error_IdAssignment);
            if (companyModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (companyModel.SoftDeletedDate != null) 
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var company = await _companiesService.AddCompanyAsync(companyModel, userLogin);

            if (company == null) return NoContent();

            var addedCompanyModel = _mapper.Map<CompanyModel>(company);

            return Ok(addedCompanyModel.CompanyId);
        }

        /// <summary>
        ///     Обновить компанию
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCompanyAsync(CompanyModel companyModel)
        {
            if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));
            if (companyModel.CompanyId <= 0) throw new UserErrorException(Resources.Error_IncorrectCompanyId);
            if (companyModel.IsDeleted) throw new UserErrorException(Resources.Error_IsDeletedTrue);
            if (companyModel.SoftDeletedDate != null) 
                throw new UserErrorException(Resources.Error_SoftDeletedDateAssignment);

            var userLogin = HttpContext.User.Identity.Name;

            var updCompany = await _companiesService.UpdateCompanyAsync(companyModel, userLogin);

            if (updCompany == null) return NoContent();

            var updCompanyModel = _mapper.Map<CompanyModel>(updCompany);

            return Ok(updCompanyModel.CompanyId);
        }

        /// <summary>
        ///     Удалить компанию
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCompanyByCompanyIdAsync(int companyId)
        {
            if (companyId <= 0) throw new UserErrorException(Resources.Error_IncorrectCompanyId);

            var userLogin = HttpContext.User.Identity.Name;

            var company = await _companiesService.DeleteCompanyByCompanyIdAsync(companyId, userLogin);

            if (company == null) return NoContent();

            var companyModel = _mapper.Map<CompanyModel>(company);

            return Ok(companyModel.CompanyId);
        }

        /// <summary>
        ///     Получить все компании
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var userLogin = HttpContext.User.Identity.Name;

            var companies = await _companiesService.GetAllCompaniesAsync(userLogin);

            if (companies == null || companies.Count == 0) return NoContent();

            var companyModels = companies.Select(c => _mapper.Map<CompanyModel>(c))
                                         .ToList();

            return Ok(companyModels);
        }

        /// <summary>
        ///     Скрыть компанию
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveCompanyByCompanyIdAsync(int companyId)
        {
            if (companyId <= 0) throw new UserErrorException(Resources.Error_IncorrectCompanyId);

            var userLogin = HttpContext.User.Identity.Name;

            var company = await _companiesService.RemoveCompanyByCompanyIdAsync(companyId, userLogin);

            if (company == null) return NoContent();

            var companyModel = _mapper.Map<CompanyModel>(company);

            return Ok(companyModel.CompanyId);
        }
    }
}