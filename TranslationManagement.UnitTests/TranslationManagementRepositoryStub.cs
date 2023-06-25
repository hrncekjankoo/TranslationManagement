using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories.Contracts;

namespace TranslationManagement.UnitTests;

public class TranslationManagementRepositoryStub : ITranslatorManagementRepository
{
    public List<Translator> TranslatorModels = new();

    public Task<Translator?> GetTranslator(int id)
    {
        return Task.FromResult(TranslatorModels.FirstOrDefault(t => t.Id == id));
    }

    public Task<Translator[]> GetTranslators()
    {
        return Task.FromResult(TranslatorModels.ToArray());
    }

    public Task<Translator[]> GetTranslatorsByName(string name)
    {
        return Task.FromResult(TranslatorModels.Where(t => t.Name == name).ToArray());
    }

    public Task<Translator> AddTranslator(Translator translator)
    {
        TranslatorModels.Add(translator);
        return Task.FromResult(translator);
    }

    public Task<Translator> UpdateTranslator(Translator translator)
    {
        var translatorInDatabase = TranslatorModels.FirstOrDefault(t => t.Id == translator.Id);
        if (translatorInDatabase == null)
        {
            throw new ArgumentException("not found");
        }
        translatorInDatabase.Name = translator.Name;
        translatorInDatabase.Status = translator.Status;
        translatorInDatabase.HourlyRate = translator.HourlyRate;
        translatorInDatabase.CreditCardNumber = translator.CreditCardNumber;
        return Task.FromResult(TranslatorModels.FirstOrDefault(t => t.Id == translator.Id))!;
    }
}