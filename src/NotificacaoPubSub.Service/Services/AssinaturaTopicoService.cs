using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NotificacaoPubSub.Service.Services
{
    public class AssinaturaTopicoService : IAssinaturaTopicoService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AssinaturaTopicoService> _logger;
        private readonly IDynamoBaseRepository<AssinaturaTopico> _assinaturaRepository;
        private readonly ITopicoService _topicoService;

        public AssinaturaTopicoService(IMapper mapper, ILogger<AssinaturaTopicoService> logger, IDynamoBaseRepository<AssinaturaTopico> assinaturaRepository, ITopicoService topicoService)
        {
            _mapper = mapper;
            _logger = logger;
            _assinaturaRepository = assinaturaRepository;
            _topicoService = topicoService;
        }

        public async Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaTopicoAsync(string clientId)
        {
            _logger.LogInformation($"Buscar assinatura topico: {clientId}");

            var condicoes = new List<ScanCondition>
            {
                new ScanCondition("ClientId", ScanOperator.Equal, clientId)
            };
            var assinatura = await _assinaturaRepository.BuscarPorScanAsync(condicoes);
            if (!assinatura.Any())
            {
                //throw new BancoMasterException("Assinatura não encontrada");
            }
            _logger.LogInformation($"Buscar assinatura topico sucesso: {JsonConvert.SerializeObject(assinatura)}");
            return assinatura;
        }

        public async Task CadastrarAssinaturaTopicoAsync(CadastrarAssinaturaTopicoRequest request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _logger.LogInformation($"Cadastrar assinatura topico: {JsonConvert.SerializeObject(request)}");

            await _topicoService.BuscarTopicoPorDescricaoAsync(request.Topico);

            var condicoes = new List<ScanCondition>
            {
                new ScanCondition("Topico", ScanOperator.Equal, request.Topico),
                new ScanCondition("IdConsumer", ScanOperator.Equal, request.IdConsumer),
                new ScanCondition("Ativo", ScanOperator.Equal, true)
            };
            var assinatura = await _assinaturaRepository.BuscarPorScanAsync(condicoes);

            if (assinatura.Any())
            {
                //throw new BancoMasterException("Cadastro já existente");
            }

            var assinaturaMap = _mapper.Map<AssinaturaTopico>(request);
            await _assinaturaRepository.SalvarAsync(assinaturaMap);
            _logger.LogInformation($"Cadastrar assinatura topico sucesso: {JsonConvert.SerializeObject(assinaturaMap)}");
        }

        public async Task DescadastrarAssinaturaTopicoAsync(string id)
        {
            _logger.LogInformation($"Descadastrar assinatura topico: {id}");

            var assinatura = await BuscarAssinaturaTopicoPorIdAsync(id);

            assinatura.Ativo = false;
            await _assinaturaRepository.SalvarAsync(assinatura);
            _logger.LogInformation($"Descadastrar assinatura topico sucesso: {JsonConvert.SerializeObject(assinatura)}");
        }

        public async Task<AssinaturaTopico> BuscarAssinaturaTopicoPorIdAsync(string id)
        {
            _logger.LogInformation($"Buscar assinatura topico por Id: {id}");

            var assinatura = await _assinaturaRepository.BuscarAsync(id);
            if (assinatura == null)
            {
                //throw new BancoMasterException($"Assinatura não encontrada com o Id: {id}");
            }
            _logger.LogInformation($"Buscar assinatura topico por Id sucesso: {JsonConvert.SerializeObject(assinatura)}");
            return assinatura;
        }

        public async Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaPorTopicoAsync(string topico)
        {
            _logger.LogInformation($"Buscar assinatura topico por Topico: {topico}");

            var condicoes = new List<ScanCondition>
            {
                new ScanCondition("Topico", ScanOperator.Equal, topico),
                new ScanCondition("Ativo", ScanOperator.Equal, true)
            };
            var assinatura = await _assinaturaRepository.BuscarPorScanAsync(condicoes);
            if (!assinatura.Any())
            {
                return null;
                //throw new BancoMasterException($"Assinatura não encontrada com o Topico: {topico}");
            }
            else
            {
                _logger.LogInformation($"Buscar assinatura topico por Topico sucesso: {JsonConvert.SerializeObject(assinatura)}");
                return assinatura;
            }
        }

        public async Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaTopicoPorCnpjAsync(string cnpj)
        {
            _logger.LogInformation($"Buscar assinatura topico por CNPJ: {cnpj}");

            var condicoes = new List<ScanCondition>
            {
                new ScanCondition("CNPJ", ScanOperator.Equal, cnpj),
                new ScanCondition("Ativo", ScanOperator.Equal, true)
            };

            var assinatura = await _assinaturaRepository.BuscarPorScanAsync(condicoes);
            if (!assinatura.Any())
            {
                return null;

                //throw new BancoMasterException($"Assinatura não encontrada com o CNPJ: {cnpj}");
            }
            else
            {
                _logger.LogInformation($"Buscar assinatura topico por CNPJ sucesso: {JsonConvert.SerializeObject(assinatura)}");
                return assinatura;
            }
        }

        public async Task<IEnumerable<AssinaturaTopico>> BuscarAssinaturaTopicoPorIdConsumerAsync(string idConsumer)
        {
            _logger.LogInformation($"Buscar assinatura topico por IdConsumer: {idConsumer}");

            var condicoes = new List<ScanCondition>
            {
                new ScanCondition("IdConsumer", ScanOperator.Equal, idConsumer),
                new ScanCondition("Ativo", ScanOperator.Equal, true)
            };
            var assinatura = await _assinaturaRepository.BuscarPorScanAsync(condicoes);

            if (!assinatura.Any())
            {
                return null;

                //throw new BancoMasterException($"Assinatura não encontrada com o IdConsumer: {idConsumer}");
            }
            else
            {
                _logger.LogInformation($"Buscar assinatura topico por IdConsumer sucesso: {JsonConvert.SerializeObject(assinatura)}");
                return assinatura;
            }
        }

        public async Task<AssinaturaTopico> AtualizarAssinaturaTopicoAsync(AtualizarAssinaturaTopicoRequest request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _logger.LogInformation($"Atualizar assinatura topico: {JsonConvert.SerializeObject(request)}");

            if (request.Topico != null)
            {
                await _topicoService.BuscarTopicoPorDescricaoAsync(request.Topico);
            }

            var assinatura = await BuscarAssinaturaTopicoPorIdAsync(request.Id);

            assinatura.ClientId = request.ClientId == null ? assinatura.ClientId : request.ClientId;
            assinatura.CNPJ = request.CNPJ == null ? assinatura.CNPJ : request.CNPJ;
            assinatura.Email = request.Email == null ? assinatura.Email : request.Email;
            assinatura.IdConsumer = request.IdConsumer == null ? assinatura.IdConsumer : request.IdConsumer;
            assinatura.Token = request.Token == null ? assinatura.Token : request.Token;
            assinatura.Topico = request.Topico == null ? assinatura.Topico : request.Topico;
            assinatura.UrlCallback = request.UrlCallback == null ? assinatura.UrlCallback : request.UrlCallback;

            await _assinaturaRepository.SalvarAsync(assinatura);
            _logger.LogInformation($"Atualizar assinatura topico sucesso: {JsonConvert.SerializeObject(assinatura)}");
            return assinatura;
        }
    }
}
