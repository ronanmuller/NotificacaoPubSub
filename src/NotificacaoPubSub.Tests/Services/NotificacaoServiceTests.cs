using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models;
using NotificacaoPubSub.Domain.Models.Configuracao;
using NotificacaoPubSub.Service.Extensions;
using NotificacaoPubSub.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NotificacaoPubSub.UnitTests.Services
{
    public class NotificacaoServiceTests
    {
        private readonly Mock<ILogger<NotificacaoService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ISqsService> _mockSqsService;
        private readonly Mock<IDynamoBaseRepository<HistoricoNotificacao>> _mockHistoricoRepo;
        private readonly Mock<IDynamoBaseRepository<AssinaturaTopico>> _mockAssinaturaRepo;
        private readonly Mock<ITopicoService> _mockTopicoService;
        private readonly ConfiguracaoSistema _configuracaoSistema;
        private readonly NotificacaoService _service;

        public NotificacaoServiceTests()
        {
            _mockLogger = new Mock<ILogger<NotificacaoService>>();
            _mockMapper = new Mock<IMapper>();
            _mockSqsService = new Mock<ISqsService>();
            _mockHistoricoRepo = new Mock<IDynamoBaseRepository<HistoricoNotificacao>>();
            _mockAssinaturaRepo = new Mock<IDynamoBaseRepository<AssinaturaTopico>>();
            _mockTopicoService = new Mock<ITopicoService>();

            // Configuração mock da fila
            _configuracaoSistema = new ConfiguracaoSistema
            {
                ConfiguracaoTI = new ConfiguracaoTI
                {
                    SqsNotificacaoPubSub = "mock-sqs"
                }
            };

            _service = new NotificacaoService(
                _mockLogger.Object,
                _mockMapper.Object,
                _mockSqsService.Object,
                _mockHistoricoRepo.Object,
                _mockAssinaturaRepo.Object,
                _configuracaoSistema,
                _mockTopicoService.Object
            );
        }

        [Fact]
        public async Task EnviarEventoAsync_With_Assinaturas_Sends_Message_And_Saves_Historico()
        {
            // Arrange
            var request = new MensagemNotificacao
            {
                Topico = "topico1",
                IdConsumer = "consumer1",
                PayloadJson = new { Key = "Value" }
            };

            var assinatura = new AssinaturaTopico
            {
                Id = "assinatura1",
                ClientId = "client1",
                Topico = "topico1",
                IdConsumer = "consumer1",
                Ativo = true
            };

            // Mock do TopicoService
            _mockTopicoService.Setup(t => t.BuscarTopicoPorDescricaoAsync(request.Topico))
                              .ReturnsAsync(new List<Domain.Models.Topico> { new Domain.Models.Topico { Descricao = request.Topico } });

            // Mock do repositório de assinaturas
            _mockAssinaturaRepo.Setup(r => r.BuscarPorScanAsync(It.IsAny<List<ScanCondition>>()))
                               .ReturnsAsync(new List<AssinaturaTopico> { assinatura });

            // Mock do SQS
            _mockSqsService.Setup(s => s.EnviarMensagemFilaAsync(It.IsAny<string>(), It.IsAny<MensagemNotificacao>()))
                           .ReturnsAsync(new MensagemResponse { Sucesso = true });

            // Mock do Mapper
            _mockMapper.Setup(m => m.Map<HistoricoNotificacao>(It.IsAny<MensagemNotificacao>()))
                       .Returns(new HistoricoNotificacao());

            // Act
            var result = await _service.EnviarEventoAsync(request);

            // Assert
            Assert.Single(result);
            _mockSqsService.Verify(s => s.EnviarMensagemFilaAsync("mock-sqs", It.IsAny<MensagemNotificacao>()), Times.Once);
            _mockHistoricoRepo.Verify(r => r.SalvarAsync(It.IsAny<HistoricoNotificacao>()), Times.Once);
        }

        [Fact]
        public async Task EnviarEventoAsync_No_Assinaturas_Returns_Empty_List()
        {
            // Arrange
            var request = new MensagemNotificacao
            {
                Topico = "topico1",
                IdConsumer = "consumer1",
                PayloadJson = new { Key = "Value" }
            };

            _mockTopicoService.Setup(t => t.BuscarTopicoPorDescricaoAsync(request.Topico))
                              .ReturnsAsync(new List<Domain.Models.Topico> { new Domain.Models.Topico { Descricao = request.Topico } });

            _mockAssinaturaRepo.Setup(r => r.BuscarPorScanAsync(It.IsAny<List<ScanCondition>>()))
                               .ReturnsAsync(new List<AssinaturaTopico>());

            // Act
            var result = await _service.EnviarEventoAsync(request);

            // Assert
            Assert.Empty(result);
            _mockSqsService.Verify(s => s.EnviarMensagemFilaAsync(It.IsAny<string>(), It.IsAny<MensagemNotificacao>()), Times.Never);
            _mockHistoricoRepo.Verify(r => r.SalvarAsync(It.IsAny<HistoricoNotificacao>()), Times.Never);
        }

        [Fact]
        public async Task EnviarEventoAsync_NullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.EnviarEventoAsync(null));
        }
    }
}
