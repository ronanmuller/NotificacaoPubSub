using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using NotificacaoPubSub.Service.Extensions;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Service.Services
{
    public class SqsService : ISqsService
    {
        public Task<MensagemResponse> EnviarMensagemFilaAsync(string sqs, MensagemNotificacao request)
        {
            throw new System.NotImplementedException();
        }
    }
}