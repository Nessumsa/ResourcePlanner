namespace ResourcePlanner.Interfaces.Adapters.CRUD
{
    public interface IReadAdapter<T, TKey>
    {
        Task<T?> ReadAsync(TKey id);
    }
}
