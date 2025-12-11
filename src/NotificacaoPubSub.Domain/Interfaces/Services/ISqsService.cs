using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Service.Extensions
{
    public interface ISqsService
    {
        Task<MensagemResponse> EnviarMensagemFilaAsync(string sqs, MensagemNotificacao request);
    }
}