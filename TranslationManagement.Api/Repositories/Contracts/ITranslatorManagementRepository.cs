using System.Threading.Tasks;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api.Repositories.Contracts
{
    public interface ITranslatorManagementRepository
    {
        Task<Translator> GetTranslator(int id);
        Task<Translator[]> GetTranslators();
        Task<Translator[]> GetTranslatorsByName(string name);
        Task<Translator> AddTranslator(Translator translator);
        Task<Translator> UpdateTranslator(Translator translator);
    }
}