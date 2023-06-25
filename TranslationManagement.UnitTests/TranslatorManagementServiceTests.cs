using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Services;
using Xunit;
using Moq;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories.Contracts;

namespace TranslationManagement.UnitTests;

public class TranslatorManagementServiceTests
{
    private const string DummyName = "dummy name";
    private const string DummyCreditCardNumber = "dummy credit card number";
    
    [Fact]
    public async Task GetsTranslators()
    {
        // Given 
        var translationManagementRepositoryStub = InitializeTranslatorManagementRepositoryStub();
        translationManagementRepositoryStub.TranslatorModels = new List<Translator>
        {
            new() { Id = 1, Name = DummyName, HourlyRate = 100, Status = TranslatorStatus.Certified, CreditCardNumber = DummyCreditCardNumber }
        };
        var service = InitializeTranslatorManagementService(translationManagementRepositoryStub);
        
        // When
        var translators = await service.GetTranslators();

        // Then
        Assert.Single(translators);
        AssertTranslatorsEqual(translators.First(), translationManagementRepositoryStub.TranslatorModels.First());
    }
    
    [Fact]
    public async Task CreatesTranslator()
    {
        // Given
        var translator = new Translator
        {
            Name = DummyName,
            Status = TranslatorStatus.Applicant,
            CreditCardNumber = DummyCreditCardNumber,
            HourlyRate = 100
        };
        var service = InitializeTranslatorManagementService();
        
        // When
        var translatorResult = await service.AddTranslator(translator);
        
        // Then
        AssertTranslatorsEqual(translator, translatorResult);
    }
    
    [Fact]
    public async Task ThrowsValidationExceptionWhenTryingToUpdateTranslatorStatusWithNullStatus()
    {
        // Given
        const int translatorId = 1;
        var translationManagementRepositoryStub = InitializeTranslatorManagementRepositoryStub();
        translationManagementRepositoryStub.TranslatorModels = new List<Translator>
        {
            new() { Id = translatorId, Name = DummyName, HourlyRate = 100, Status = TranslatorStatus.Certified, CreditCardNumber = DummyCreditCardNumber }
        };
        var service = InitializeTranslatorManagementService(translationManagementRepositoryStub);
        
        // When
        var exception = await Record.ExceptionAsync(async () => await service.UpdateTranslatorStatus(translatorId, null));

        // Then
        Assert.IsAssignableFrom<ValidationException>(exception);
    }

    private static void AssertTranslatorsEqual(Translator translatorRequest, Translator translatorResult)
    {
        Assert.Equal(translatorRequest.Name, translatorResult.Name);
        Assert.Equal(translatorRequest.HourlyRate, translatorResult.HourlyRate);
        Assert.Equal(translatorRequest.CreditCardNumber, translatorResult.CreditCardNumber);
        Assert.Equal(translatorRequest.Status, translatorResult.Status);
    }

    private static TranslationManagementRepositoryStub InitializeTranslatorManagementRepositoryStub()
    {
        return new TranslationManagementRepositoryStub();
    }
    
    private static TranslatorManagementService InitializeTranslatorManagementService(ITranslatorManagementRepository? translatorManagementRepository = null)
    {
        var logger = new Mock<ILogger<TranslatorManagementService>>().Object;
        return new TranslatorManagementService(translatorManagementRepository ?? new TranslationManagementRepositoryStub(), logger);
    }
}