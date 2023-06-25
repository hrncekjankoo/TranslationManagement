using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories.Contracts;
using TranslationManagement.Api.Services.Contracts;

namespace TranslationManagement.Api.Services
{
    public class TranslatorManagementService : ITranslatorManagementService
    {
        private readonly ITranslatorManagementRepository _translatorManagementRepository;
        private readonly ILogger<TranslatorManagementService> _logger;

        public TranslatorManagementService(ITranslatorManagementRepository translatorManagementRepository, ILogger<TranslatorManagementService> logger)
        {
            _translatorManagementRepository = translatorManagementRepository;
            _logger = logger;
        }
        
        public async Task<Translator[]> GetTranslators()
        {
            return await _translatorManagementRepository.GetTranslators();
        }
        
        public async Task<Translator[]> GetTranslatorsByName(string name)
        {
            return await _translatorManagementRepository.GetTranslatorsByName(name);
        }
        
        public async Task<Translator> AddTranslator(Translator translator)
        {
            return await _translatorManagementRepository.AddTranslator(translator);
        }
        
        public async Task<Translator> UpdateTranslatorStatus(int id, TranslatorStatus? newStatus)
        {
            _logger.LogInformation("User status update request: " + newStatus + " for user " + id);
            if (newStatus == null)
            {
                throw new ValidationException("status is empty");
            }

            var translator = await _translatorManagementRepository.GetTranslator(id);
            translator.Status = newStatus!.Value;

            return await _translatorManagementRepository.UpdateTranslator(translator);
        }
    }
}