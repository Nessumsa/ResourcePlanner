namespace ResourcePlanner.Interfaces.Adapters.CRUD
{
    public interface ICreateAdapter<T>
    {
        Task<bool> CreateAsync(T entity);
    }
}
