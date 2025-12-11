using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Domain.Interfaces.Services
{
    public interface ITopicoService
    {
        Task<IEnumerable<Topico>> BuscarTodosTopicosAsync();
        Task<IEnumerable<Topico>> BuscarTopicoPorDescricaoAsync(string descricao);
        Task<Topico> BuscarTopicoPorIdAsync(string id);
        Task CadastrarTopicoAsync(CadastrarTopicoRequest request);
        Task DescadastrarTopicoAsync(string id);
        Task AtualizarTopicoAsync(AtualizarTopicoRequest request);

    }
}
