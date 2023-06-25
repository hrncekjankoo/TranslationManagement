using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories.Contracts;

namespace TranslationManagement.Api.Repositories
{
    public class TranslatorManagementRepository : ITranslatorManagementRepository
    {
        private readonly AppDbContext _context;
        
        public TranslatorManagementRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Translator> GetTranslator(int id)
        {
            return await _context.Translators.FirstOrDefaultAsync(t => t.Id == id);
        }
        
        public async Task<Translator[]> GetTranslators()
        {
            return await _context.Translators.ToArrayAsync();
        }
        
        public async Task<Translator[]> GetTranslatorsByName(string name)
        {
            return await _context.Translators.Where(t => t.Name == name).ToArrayAsync();
        }

        public async Task<Translator> AddTranslator(Translator translator)
        {
            _context.Translators.Add(translator);
            await _context.SaveChangesAsync();
            return await GetTranslator(translator.Id);
        }
        
        public async Task<Translator> UpdateTranslator(Translator translator)
        {
            await _context.SaveChangesAsync();

            return await GetTranslator(translator.Id);
        }
    }
}