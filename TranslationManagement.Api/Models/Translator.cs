namespace TranslationManagement.Api.Models
{
    public class Translator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal HourlyRate { get; set; }
        public TranslatorStatus Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}