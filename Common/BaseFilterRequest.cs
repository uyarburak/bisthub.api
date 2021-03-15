namespace BistHub.Api.Common
{
    public class BaseFilterRequest<T>
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public T Filter { get; set; }
        public int Offset => Page * ItemsPerPage;
    }
}
