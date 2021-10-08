using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Abstract;

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
            try
            {
                if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

                var userLogin = HttpContext.User.Identity.Name;

                var company = await _companiesService.GetCompanyByCompanyIdAsync(companyId, userLogin);

                if (company == null || company.IsDeleted) return NotFound();

                var companyModel = _mapper.Map<CompanyModel>(company);

                return Ok(companyModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
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
            try
            {
                if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));

                var userLogin = HttpContext.User.Identity.Name;

                var company = await _companiesService.AddCompanyAsync(companyModel, userLogin);

                if (company == null) return NotFound();

                var addedCompanyModel = _mapper.Map<CompanyModel>(company);

                return Ok(addedCompanyModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///     Обновить компанию
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDriverAsync(CompanyModel companyModel)
        {
            try
            {
                if (companyModel == null) throw new ArgumentNullException(nameof(companyModel));

                var userLogin = HttpContext.User.Identity.Name;

                var updCompany = await _companiesService.UpdateCompanyAsync(companyModel, userLogin);

                if (updCompany == null) return NotFound();

                var updCompanyModel = _mapper.Map<CompanyModel>(updCompany);

                return Ok(updCompanyModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
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
            try
            {
                if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

                var userLogin = HttpContext.User.Identity.Name;

                var company = await _companiesService.DeleteCompanyByCompanyIdAsync(companyId, userLogin);

                if (company == null) return NotFound();

                var companyModel = _mapper.Map<CompanyModel>(company);

                return Ok(companyModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
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
            try
            {
                var userLogin = HttpContext.User.Identity.Name;

                var companies = await _companiesService.GetAllCompaniesAsync(userLogin);

                if (companies == null) return NotFound();

                var companyModels = companies.Select(c => _mapper.Map<CompanyModel>(c))
                                             .ToList();

                return Ok(companyModels);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
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
            try
            {
                if (companyId <= 0) throw new ArgumentOutOfRangeException(nameof(companyId));

                var userLogin = HttpContext.User.Identity.Name;

                var company = await _companiesService.RemoveCompanyByCompanyIdAsync(companyId, userLogin);

                if (company == null) return NotFound();

                var companyModel = _mapper.Map<CompanyModel>(company);

                return Ok(companyModel);
            }
            catch (Exception e)
            {
                if (e is Npgsql.PostgresException) return BadRequest("Ошибка работы БД");
                return BadRequest(e.Message);
            }
        }
    }
}