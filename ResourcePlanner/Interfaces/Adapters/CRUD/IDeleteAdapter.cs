namespace ResourcePlanner.Interfaces.Adapters.CRUD
{
    public interface IDeleteAdapter<TKey>
    {
        Task<bool> DeleteAsync(TKey id);
    }
}
