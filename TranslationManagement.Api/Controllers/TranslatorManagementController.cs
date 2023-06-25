using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Services.Contracts;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/TranslatorsManagement/[action]")]
    public class TranslatorManagementController : ControllerBase
    {
        private readonly ITranslatorManagementService _translatorManagementService;

        public TranslatorManagementController(ITranslatorManagementService translatorManagementService)
        {
            _translatorManagementService = translatorManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTranslators()
        {
            var translators = await _translatorManagementService.GetTranslators();
            return Ok(translators);
        }

        [HttpGet]
        public async Task<IActionResult> GetTranslatorsByName(string name)
        {
            var translators = await _translatorManagementService.GetTranslatorsByName(name);
            return Ok(translators);
        }

        [HttpPost]
        public async Task<IActionResult> AddTranslator(Translator translator)
        {
            var createdTranslator = await _translatorManagementService.AddTranslator(translator);
            return Ok(createdTranslator);
        }
        
        [HttpPatch]
        public async Task<IActionResult> UpdateTranslatorStatus(int id, TranslatorStatus? newStatus = null)
        {
            var updatedTranslator = await _translatorManagementService.UpdateTranslatorStatus(id, newStatus);
            return Ok(updatedTranslator);
        }
    }
}