namespace BuildingBlocks.Domain.Exceptions
{
    // İstenen veri bulunamadığında kullanılır
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
