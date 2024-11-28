using ResourcePlanner.Interfaces.Adapters.CRUD;

namespace ResourcePlanner.Interfaces.Adapters
{
    public interface IInstitutionAdapter<T, TKey> :
        IReadAdapter<T, TKey>,
        IUpdateAdapter<T>
    {
    }
}
