using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api.Services.Contracts
{
    public interface ITranslationJobService
    {
        Task<TranslationJob[]> GetJobs();
        Task<TranslationJob> CreateJob(TranslationJob job);
        Task<TranslationJob> CreateJobWithFile(IFormFile file, string customer);
        Task<TranslationJob> UpdateJobStatus(int jobId, JobStatus? newStatus);
        Task<TranslationJob> AssignTranslator(int jobId, int translatorId);
    }
}