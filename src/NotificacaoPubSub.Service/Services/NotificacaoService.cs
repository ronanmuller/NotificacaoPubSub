using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models;
using NotificacaoPubSub.Domain.Models.Configuracao;
using NotificacaoPubSub.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Service.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly ILogger<NotificacaoService> _logger;
        private readonly IMapper _mapper;
        private readonly ISqsService _sqsService;
        private readonly IDynamoBaseRepository<HistoricoNotificacao> _historicoRepository;
        private readonly IDynamoBaseRepository<AssinaturaTopico> _assinaturaRepository;
        private readonly ITopicoService _topicoService;
        private readonly ConfiguracaoSistema _configuracaoSistema;

        public NotificacaoService(
            ILogger<NotificacaoService> logger,
            IMapper mapper,
            ISqsService sqsService,
            IDynamoBaseRepository<HistoricoNotificacao> historicoRepository,
            IDynamoBaseRepository<AssinaturaTopico> assinaturaRepository,
            ConfiguracaoSistema configuracaoSistema,
            ITopicoService topicoService)
        {
            _logger = logger;
            _mapper = mapper;
            _sqsService = sqsService;
            _historicoRepository = historicoRepository;
            _assinaturaRepository = assinaturaRepository;
            _configuracaoSistema = configuracaoSistema;
            _topicoService = topicoService;
        }

        public async Task<IEnumerable<MensagemResponse>> EnviarEventoAsync(MensagemNotificacao request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            await _topicoService.BuscarTopicoPorDescricaoAsync(request.Topico);

            List<ScanCondition> scanList = new List<ScanCondition>
                {
                    new ScanCondition("IdConsumer", ScanOperator.Equal, request.IdConsumer ),
                    new ScanCondition("Topico", ScanOperator.Equal, request.Topico ),
                    new ScanCondition("Ativo", ScanOperator.Equal, true)
                };

            var retornoFila = new List<MensagemResponse>();

            var retornoDynamo = await _assinaturaRepository.BuscarPorScanAsync(scanList);
            if (retornoDynamo.Any())
            {
                foreach (var assinatura in retornoDynamo)
                {
                    request.Id = Guid.NewGuid().ToString();
                    request.Payload = JsonSerializer.Serialize(request.PayloadJson);
                    request.AssinaturaTopico = assinatura;

                    _logger.LogInformation($"Enviar evento: {JsonSerializer.Serialize(request)}");

                    var sqs = _configuracaoSistema.ConfiguracaoTI.SqsNotificacaoPubSub;
                    var envioFila = await _sqsService.EnviarMensagemFilaAsync(sqs, request);

                    await SalvarHistoricoNotificacaoAsync(request);

                    retornoFila.Add(envioFila);
                }
            }
            else
            {
                _logger.LogInformation("Assinatura não encontrada");
            }
            return retornoFila;
        }

        private async Task SalvarHistoricoNotificacaoAsync(MensagemNotificacao request)
        {
            var historico = _mapper.Map<HistoricoNotificacao>(request);
            historico.NotificacaoPubSubId = request.AssinaturaTopico.Id;
            historico.ClientId = request.AssinaturaTopico.ClientId;
            await _historicoRepository.SalvarAsync(historico);
        }
    }
}
