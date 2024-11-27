namespace ResourcePlanner.Interfaces.Adapters.CRUD
{
    public interface IUpdateAdapter<T>
    {
        Task<bool> UpdateAsync(T entity);
    }
}
