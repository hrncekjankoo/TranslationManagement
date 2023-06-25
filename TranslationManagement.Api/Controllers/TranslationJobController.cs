using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Services.Contracts;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly ITranslationJobService _translationJobService;

        public TranslationJobController(ITranslationJobService translationJobService)
        {
            _translationJobService = translationJobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _translationJobService.GetJobs();
            return Ok(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob(TranslationJob job)
        {
            var createdJob = await _translationJobService.CreateJob(job);
            return Ok(createdJob);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobWithFile(IFormFile file, string customer)
        {
            var createdJob = await _translationJobService.CreateJobWithFile(file, customer);
            return Ok(createdJob);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateJobStatus(int jobId, JobStatus? newStatus = null)
        {
            var updatedJob = await _translationJobService.UpdateJobStatus(jobId, newStatus);
            return Ok(updatedJob);
        }
        
        [HttpPatch]
        public async Task<IActionResult> AssignTranslator(int jobId, int translatorId)
        {
            var updatedJob = await _translationJobService.AssignTranslator(jobId, translatorId);
            return Ok(updatedJob);
        }
    }
}