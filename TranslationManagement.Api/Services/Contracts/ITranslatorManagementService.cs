using System.Threading.Tasks;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api.Services.Contracts
{
    public interface ITranslatorManagementService
    {
        Task<Translator[]> GetTranslators();
        Task<Translator[]> GetTranslatorsByName(string name);
        Task<Translator> AddTranslator(Translator translator);
        Task<Translator> UpdateTranslatorStatus(int id, TranslatorStatus? newStatus = null);
    }
}