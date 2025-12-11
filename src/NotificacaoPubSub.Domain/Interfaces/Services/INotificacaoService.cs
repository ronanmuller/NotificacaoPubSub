using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Domain.Interfaces.Services
{
    public interface INotificacaoService
    {
        Task<IEnumerable<MensagemResponse>> EnviarEventoAsync(MensagemNotificacao request);
    }
}
