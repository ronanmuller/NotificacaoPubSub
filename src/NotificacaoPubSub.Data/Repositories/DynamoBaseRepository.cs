using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Data.Repositories
{
    public class DynamoBaseRepository<T> : IDynamoBaseRepository<T>
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public DynamoBaseRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
        }

        public async Task SalvarAsync(T item)
        {
            await _context.SaveAsync(item);
        }

        public async Task<T> BuscarAsync(object id)
        {
            return await _context.LoadAsync<T>(id);
        }

        public async Task<IEnumerable<T>> BuscarPorScanAsync(List<ScanCondition> condicoes)
        {
            return await _context.ScanAsync<T>(condicoes).GetRemainingAsync();
        }

        public async Task<IEnumerable<T>> BuscarTodosAsync()
        {
            var condicoes = new List<ScanCondition>();
            return await _context.ScanAsync<T>(condicoes).GetRemainingAsync();
        }

        public async Task DeletarAsync(T item)
        {
            await _context.DeleteAsync(item);
        }
    }
}
