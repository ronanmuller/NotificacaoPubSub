using Amazon.SQS;
using Amazon.SQS.Model;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models;
using NotificacaoPubSub.Service.Extensions;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Service.Services
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;

        public SqsService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task<MensagemResponse> EnviarMensagemFilaAsync(string sqsQueueUrl, MensagemNotificacao request)
        {
            if (string.IsNullOrWhiteSpace(sqsQueueUrl))
                throw new ArgumentNullException(nameof(sqsQueueUrl));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Serializa a mensagem
            string messageBody = JsonSerializer.Serialize(request);

            // Cria o request do SQS
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = sqsQueueUrl,
                MessageBody = messageBody
            };

            // Envia a mensagem
            var response = await _sqsClient.SendMessageAsync(sendMessageRequest);

            // Retorna resultado simplificado
            return new MensagemResponse
            {
                Sucesso = response.HttpStatusCode == System.Net.HttpStatusCode.OK,
                MessageId = response.MessageId
            };
        }
    }
}
