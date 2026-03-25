namespace BuildingBlocks.Domain.Exceptions
{
    // İş kuralı ihlallerini temsil eder
    public sealed class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message)
            : base(message)
        {
        }
    }
}
