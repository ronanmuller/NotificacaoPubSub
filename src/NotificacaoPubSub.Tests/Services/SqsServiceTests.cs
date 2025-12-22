using Amazon.SQS;
using Amazon.SQS.Model;
using Moq;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using NotificacaoPubSub.Service.Services;
using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NotificacaoPubSub.UnitTests.Services
{
    public class SqsServiceTests
    {
        private readonly Mock<IAmazonSQS> _sqsClientMock;
        private readonly SqsService _service;

        public SqsServiceTests()
        {
            _sqsClientMock = new Mock<IAmazonSQS>();
            _service = new SqsService(_sqsClientMock.Object);
        }

        // =========================
        // 1️⃣ Validação de parâmetros
        // =========================

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task EnviarMensagemFilaAsync_Deve_Lancar_Exception_Quando_QueueUrl_Invalida(string queueUrl)
        {
            var request = new MensagemNotificacao();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.EnviarMensagemFilaAsync(queueUrl, request));
        }

        [Fact]
        public async Task EnviarMensagemFilaAsync_Deve_Lancar_Exception_Quando_Request_Nulo()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.EnviarMensagemFilaAsync("https://sqs.queue", null));
        }

        [Fact]
        public async Task EnviarMensagemFilaAsync_Deve_Enviar_Mensagem_Com_Sucesso()
        {
            // Arrange
            var queueUrl = "https://sqs.us-east-1.amazonaws.com/123/test";

            var request = new MensagemNotificacao
            {
                Id = "1",
                Topico = "topico-teste"
            };

            var expectedJson = JsonSerializer.Serialize(request);

            var awsResponse = new SendMessageResponse
            {
                HttpStatusCode = HttpStatusCode.OK,
                MessageId = "msg-123"
            };

            _sqsClientMock
                .Setup(s => s.SendMessageAsync(
                    It.IsAny<SendMessageRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(awsResponse);

            // Act
            var result = await _service.EnviarMensagemFilaAsync(queueUrl, request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.Equal("msg-123", result.MessageId);

            _sqsClientMock.Verify(s =>
                s.SendMessageAsync(
                    It.Is<SendMessageRequest>(req =>
                        req.QueueUrl == queueUrl &&
                        req.MessageBody == expectedJson
                    ),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // =========================
        // 3️⃣ Status diferente de OK
        // =========================

        [Fact]
        public async Task EnviarMensagemFilaAsync_Deve_Retornar_Sucesso_False_Quando_Status_Nao_OK()
        {
            // Arrange
            var request = new MensagemNotificacao();

            _sqsClientMock
                .Setup(s => s.SendMessageAsync(
                    It.IsAny<SendMessageRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SendMessageResponse
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    MessageId = "erro-456"
                });

            // Act
            var result = await _service.EnviarMensagemFilaAsync("queue-url", request);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Equal("erro-456", result.MessageId);
        }

        // =========================
        // 4️⃣ Exceção do SDK
        // =========================

        [Fact]
        public async Task EnviarMensagemFilaAsync_Deve_Propagar_Exception_Do_SDK()
        {
            // Arrange
            _sqsClientMock
                .Setup(s => s.SendMessageAsync(
                    It.IsAny<SendMessageRequest>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new AmazonSQSException("Falha no SQS"));

            // Act & Assert
            await Assert.ThrowsAsync<AmazonSQSException>(() =>
                _service.EnviarMensagemFilaAsync("queue-url", new MensagemNotificacao()));
        }
    }
}
