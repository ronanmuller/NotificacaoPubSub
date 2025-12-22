using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Service.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Amazon.DynamoDBv2.DataModel;

namespace NotificacaoPubSub.UnitTests.Services
{
    public class AssinaturaTopicoServiceTests
    {
        private readonly Mock<IDynamoBaseRepository<AssinaturaTopico>> _mockRepo;
        private readonly Mock<ITopicoService> _mockTopicoService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<AssinaturaTopicoService>> _mockLogger;
        private readonly AssinaturaTopicoService _service;

        public AssinaturaTopicoServiceTests()
        {
            _mockRepo = new Mock<IDynamoBaseRepository<AssinaturaTopico>>();
            _mockTopicoService = new Mock<ITopicoService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<AssinaturaTopicoService>>();

            _service = new AssinaturaTopicoService(
                _mockMapper.Object,
                _mockLogger.Object,
                _mockRepo.Object,
                _mockTopicoService.Object
            );
        }

        [Fact]
        public async Task BuscarAssinaturaTopicoAsync_Returns_Assinaturas()
        {
            // Arrange
            string clientId = "client123";
            var mockData = new List<AssinaturaTopico> { new AssinaturaTopico { ClientId = clientId } };
            _mockRepo.Setup(r => r.BuscarPorScanAsync(It.IsAny<List<ScanCondition>>()))
                     .ReturnsAsync(mockData);

            // Act
            var result = await _service.BuscarAssinaturaTopicoAsync(clientId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(clientId, result.First().ClientId);
        }

        [Fact]
        public async Task CadastrarAssinaturaTopicoAsync_Calls_RepositorySave()
        {
            // Arrange
            var request = new CadastrarAssinaturaTopicoRequest
            {
                Topico = "topico1",
                IdConsumer = "consumer1"
            };

            _mockRepo.Setup(r => r.BuscarPorScanAsync(It.IsAny<List<ScanCondition>>()))
                     .ReturnsAsync(new List<AssinaturaTopico>());

            _mockMapper.Setup(m => m.Map<AssinaturaTopico>(It.IsAny<CadastrarAssinaturaTopicoRequest>()))
                       .Returns(new AssinaturaTopico { Topico = request.Topico, IdConsumer = request.IdConsumer });

            // Corrigido: retornar IEnumerable<Topico> para BuscarTopicoPorDescricaoAsync
            _mockTopicoService.Setup(t => t.BuscarTopicoPorDescricaoAsync(request.Topico))
                              .ReturnsAsync(new List<Topico> { new Topico { Descricao = request.Topico } });

            // Act
            await _service.CadastrarAssinaturaTopicoAsync(request);

            // Assert
            _mockRepo.Verify(r => r.SalvarAsync(It.Is<AssinaturaTopico>(
                a => a.Topico == request.Topico && a.IdConsumer == request.IdConsumer
            )), Times.Once);
        }

        [Fact]
        public async Task DescadastrarAssinaturaTopicoAsync_Sets_Ativo_False()
        {
            // Arrange
            string id = "123";
            var assinatura = new AssinaturaTopico { Ativo = true };
            _mockRepo.Setup(r => r.BuscarAsync(id)).ReturnsAsync(assinatura);

            // Act
            await _service.DescadastrarAssinaturaTopicoAsync(id);

            // Assert
            Assert.False(assinatura.Ativo);
            _mockRepo.Verify(r => r.SalvarAsync(assinatura), Times.Once);
        }
    }
}
