namespace ResourcePlanner.Interfaces.Adapters.CRUD
{
    public interface ICrudAdapter<T, TKey> : 
        IReadAdapter<T, TKey>,
        IReadAllAdapter<T, TKey>,
        ICreateAdapter<T>,
        IUpdateAdapter<T>,
        IDeleteAdapter<TKey>
    {}
}
