using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;

namespace ResourcePlanner.Interfaces.Adapters
{
    public interface IErrorReportAdapter : 
        IReadAllAdapter<ErrorReport, string>,
        IUpdateAdapter<ErrorReport>
    {
        Task<IEnumerable<ErrorReport>> ReadAllActiveAsync(string institutionId);
    }
}
