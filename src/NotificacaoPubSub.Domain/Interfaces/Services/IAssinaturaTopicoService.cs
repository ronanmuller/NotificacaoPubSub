using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Domain.Interfaces.Services
{
    public interface IAssinaturaTopicoService
    {
        Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaTopicoAsync(string clientId);
        Task CadastrarAssinaturaTopicoAsync(CadastrarAssinaturaTopicoRequest request);
        Task DescadastrarAssinaturaTopicoAsync(string id);
        Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaTopicoPorCnpjAsync(string cnpj);
        Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaTopicoPorIdConsumerAsync(string idConsumer);
        Task<AssinaturaTopico> BuscarAssinaturaTopicoPorIdAsync(string id);
        Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaPorTopicoAsync(string topico);
        Task<AssinaturaTopico> AtualizarAssinaturaTopicoAsync(AtualizarAssinaturaTopicoRequest request);

    }
}
