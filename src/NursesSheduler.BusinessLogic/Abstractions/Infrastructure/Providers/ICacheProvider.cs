namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers
{
    public interface ICacheProvider<TObject, TKey> where TObject : class
    {
        Task<TObject> GetCachedDataAsync(TKey id);
        void InvalidateCache(TKey id);
    }
}
