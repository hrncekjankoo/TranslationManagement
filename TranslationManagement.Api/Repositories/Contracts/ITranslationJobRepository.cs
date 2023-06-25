using System.Threading.Tasks;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api.Repositories.Contracts
{
    public interface ITranslationJobRepository
    {
        Task<TranslationJob> GetJob(int id);
        Task<TranslationJob[]> GetJobs();
        Task<TranslationJob> CreateJob(TranslationJob job);
        Task<TranslationJob> UpdateJob(TranslationJob job);
    }
}