using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories.Contracts;

namespace TranslationManagement.Api.Repositories
{
    public class TranslationJobRepository : ITranslationJobRepository
    {
        private readonly AppDbContext _context;
        
        public TranslationJobRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<TranslationJob> GetJob(int id)
        {
            return await _context.TranslationJobs.FirstOrDefaultAsync(j => j.Id == id);
        }
        
        public async Task<TranslationJob[]> GetJobs()
        {
            return await _context.TranslationJobs.ToArrayAsync();
        }

        public async Task<TranslationJob> CreateJob(TranslationJob job)
        {
            var addedJob = _context.TranslationJobs.Add(job);
            await _context.SaveChangesAsync();
            return await GetJob(addedJob.Entity.Id);
        }

        public async Task<TranslationJob> UpdateJob(TranslationJob job)
        {
            await _context.SaveChangesAsync();
            return await GetJob(job.Id);
        }
    }
}