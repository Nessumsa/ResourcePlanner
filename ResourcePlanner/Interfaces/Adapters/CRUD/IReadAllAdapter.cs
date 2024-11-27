namespace ResourcePlanner.Interfaces.Adapters.CRUD
{
    public interface IReadAllAdapter<T, TKey>
    {
        Task<IEnumerable<T>?> ReadAllAsync(TKey id);
    }
}
