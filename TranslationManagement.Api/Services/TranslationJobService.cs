using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories.Contracts;
using TranslationManagement.Api.Services.Contracts;

namespace TranslationManagement.Api.Services
{
    public class TranslationJobService : ITranslationJobService
    {
        private const double PricePerCharacter = 0.01;
        
        private readonly ITranslationJobRepository _translationJobRepository;
        private readonly ITranslatorManagementRepository _translatorManagementRepository;
        private readonly ILogger<TranslationJobService> _logger;
        
        public TranslationJobService(
            ITranslationJobRepository translationJobRepository, 
            ITranslatorManagementRepository translatorManagementRepository, 
            ILogger<TranslationJobService> logger)
        {
            _translationJobRepository = translationJobRepository;
            _translatorManagementRepository = translatorManagementRepository;
            _logger = logger;
        }
        
        public async Task<TranslationJob[]> GetJobs()
        {
            return await _translationJobRepository.GetJobs();
        }

        public async Task<TranslationJob> CreateJob(TranslationJob job)
        {
            job.Status = JobStatus.New;
            SetPrice(job);
            var createdJob = await _translationJobRepository.CreateJob(job);
            var notificationSvc = new UnreliableNotificationService();
            while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
            {
            }

            _logger.LogInformation("New job notification sent");

            return createdJob;
        }
        
        public async Task<TranslationJob> CreateJobWithFile(IFormFile file, string customer)
        {
            var reader = new StreamReader(file.OpenReadStream());
            string content;

            if (file.FileName.EndsWith(".txt"))
            {
                content = await reader.ReadToEndAsync();
            }
            else if (file.FileName.EndsWith(".xml"))
            {
                var xDoc = XDocument.Parse(await reader.ReadToEndAsync());
                if (xDoc.Root?.Element("Content") == null || xDoc.Root?.Element("Customer") == null)
                {
                    throw new InvalidDataException("mission customer or content");
                    
                }
                content = xDoc.Root.Element("Content")!.Value;
                customer = xDoc.Root.Element("Customer")!.Value.Trim();
            }
            else
            {
                throw new NotSupportedException("unsupported file");
            }

            var newJob = new TranslationJob
            {
                OriginalContent = content,
                TranslatedContent = "",
                CustomerName = customer,
            };

            SetPrice(newJob);

            return await _translationJobRepository.CreateJob(newJob);
        }
        
        public async Task<TranslationJob> UpdateJobStatus(int jobId, JobStatus? newStatus = null)
        {
            _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId);
            if (newStatus == null)
            {
                throw new ValidationException("status is empty");
            }

            var job = await _translationJobRepository.GetJob(jobId);
            
            if ((job.Status == JobStatus.New && newStatus == JobStatus.Completed) ||
                job.Status == JobStatus.Completed || newStatus == JobStatus.New)
            {
                throw new ValidationException("invalid status change");
            }

            job.Status = newStatus!.Value;
            return await _translationJobRepository.UpdateJob(job);
        }
        
        public async Task<TranslationJob> AssignTranslator(int jobId, int translatorId)
        {
            var job = await _translationJobRepository.GetJob(jobId);
            var translator = await _translatorManagementRepository.GetTranslator(translatorId);
            
            if (translator.Status != TranslatorStatus.Certified) throw new ValidationException("not certified translator");
            
            job.TranslatorId = translatorId;
            job.Translator = translator;
            return await _translationJobRepository.UpdateJob(job);

        }
        
        private static void SetPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }
    }
}