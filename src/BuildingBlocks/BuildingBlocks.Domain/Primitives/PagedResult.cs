namespace BuildingBlocks.Domain.Primitives
{
    // Sayfalı veri sonucunu temsil eder
    public sealed class PagedResult<T>
    {
        // Sayfadaki kayıtlar
        public IReadOnlyCollection<T> Items { get; }

        // Toplam kayıt sayısı
        public int TotalCount { get; }

        // Mevcut sayfa numarası
        public int Page { get; }

        // Sayfa başına kayıt sayısı
        public int PageSize { get; }

        public PagedResult(IReadOnlyCollection<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}
