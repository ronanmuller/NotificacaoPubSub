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
    public class TopicoService : ITopicoService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TopicoService> _logger;
        private readonly IDynamoBaseRepository<Topico> _topicoRepository;

        public TopicoService(IMapper mapper, ILogger<TopicoService> logger, IDynamoBaseRepository<Topico> topicoRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _topicoRepository = topicoRepository;
        }

        public async Task AtualizarTopicoAsync(AtualizarTopicoRequest request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _logger.LogInformation($"Atualizar topico: {JsonConvert.SerializeObject(request)}");

            var topico = await BuscarTopicoPorIdAsync(request.Id);

            topico.Descricao = request.Descricao;
            await _topicoRepository.SalvarAsync(topico);
            _logger.LogInformation($"Atualizar topico sucesso: {JsonConvert.SerializeObject(topico)}");
        }

        public async Task<IEnumerable<Topico>> BuscarTodosTopicosAsync()
        {
            _logger.LogInformation($"Buscar todos topicos");

            var topico = await _topicoRepository.BuscarTodosAsync();
            if (!topico.Any())
            {
                //throw new BancoMasterException("Topicos não encontrados");
            }
            _logger.LogInformation($"Buscar todos topicos sucesso: {JsonConvert.SerializeObject(topico)}");
            return topico;
        }

        public async Task<IEnumerable<Topico>> BuscarTopicoPorDescricaoAsync(string descricao)
        {
            _logger.LogInformation($"Buscar topico por Descrição: {descricao}");
            var condicoes = new List<ScanCondition>
                {
                    new ScanCondition("Descricao", ScanOperator.Equal, descricao)
                };
            var topico = await _topicoRepository.BuscarPorScanAsync(condicoes);
            if (!topico.Any())
            {
                //throw new BancoMasterException($"Topico não encontrado com Descrição: {descricao}");
            }
            _logger.LogInformation($"Buscar topico por Descrição sucesso: {JsonConvert.SerializeObject(topico)}");
            return topico;
        }

        public async Task<Topico> BuscarTopicoPorIdAsync(string id)
        {
            _logger.LogInformation($"Buscar topico por Id: {id}");

            var topico = await _topicoRepository.BuscarAsync(id);
            if (topico == null)
            {
                //throw new BancoMasterException($"Topico não encontrado com Id: {id}");
            }
            _logger.LogInformation($"Buscar topico por Id sucesso: {JsonConvert.SerializeObject(topico)}");
            return topico;
        }

        public async Task CadastrarTopicoAsync(CadastrarTopicoRequest request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));
            _logger.LogInformation($"Cadastrar topico: {JsonConvert.SerializeObject(request)}");
            var condicoes = new List<ScanCondition>
                {
                    new ScanCondition("Descricao", ScanOperator.Equal, request.Descricao)
                };
            var topico = await _topicoRepository.BuscarPorScanAsync(condicoes);
            if (topico.FirstOrDefault() != null)
            {
                //throw new BancoMasterException("Topico já cadastrado");
            }
            var topicoMap = _mapper.Map<Topico>(request);
            await _topicoRepository.SalvarAsync(topicoMap);
            _logger.LogInformation($"Cadastrar assinatura topico sucesso: {JsonConvert.SerializeObject(topicoMap)}");
        }

        public async Task DescadastrarTopicoAsync(string id)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));
            var topico = await BuscarTopicoPorIdAsync(id);
            _logger.LogInformation($"Descadastrar topico: {JsonConvert.SerializeObject(topico)}");
            await _topicoRepository.DeletarAsync(topico);
            _logger.LogInformation($"Descadastrar topico sucesso: {JsonConvert.SerializeObject(topico)}");
        }

    }
}
