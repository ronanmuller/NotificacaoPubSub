using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Domain.Interfaces.Repositories
{
    public interface IDynamoBaseRepository<T>
    {
        Task<IEnumerable<T>> BuscarTodosAsync();
        Task<IEnumerable<T>> BuscarPorScanAsync(List<ScanCondition> condicoes);
        Task<T> BuscarAsync(object id);
        Task SalvarAsync(T item);
        Task DeletarAsync(T item);
    }
}
