using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssinaturaTopicoController : ControllerBase
    {
        private readonly ILogger<AssinaturaTopicoController> _logger;
        private readonly IAssinaturaTopicoService _assinaturaService;

        public AssinaturaTopicoController(
            ILogger<AssinaturaTopicoController> logger,
            IAssinaturaTopicoService assinaturaService)
        {
            _logger = logger;
            _assinaturaService = assinaturaService;
        }

        /// <summary>
        /// Cadastrar assinatura topico
        /// </summary>
        /// <param name="request">Objeto de cadastro assinatura topico</param>
        /// <returns></returns>
        /// <response code="201">Assinatura cadastrada com suceso</response>
        /// <response code="400">Nao foi possivel cadastrar assinatura</response>    
        /// <response code="500">Erro interno na api</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CadastrarAssinaturaTopico([FromBody][Required] CadastrarAssinaturaTopicoRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Inserção com erro: {request}", JsonConvert.SerializeObject(request));
                return BadRequest(ModelState);
            }

            await _assinaturaService.CadastrarAssinaturaTopicoAsync(request);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Descadastrar assinatura topico
        /// </summary>
        /// <param name="id">Id para descadastro da assinatura</param>
        /// <returns></returns>
        /// <response code="204">Assinatura descadastrada com sucesso</response>
        /// <response code="400">Nao foi possível descadastrar assinatura</response>    
        /// <response code="500">Erro interno na api</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DescadastrarAssinaturaTopico([Required] string id)
        {
            await _assinaturaService.DescadastrarAssinaturaTopicoAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Consultar assinatura topico por ClientId
        /// </summary>
        /// <param name="clientId">Cliente Id</param>
        /// <returns>Assinatura encontrada via clienteId</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorClientId/{clientId}")]
        [ProducesResponseType(typeof(IEnumerable<AssinaturaTopico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssinaturaTopico>>> BuscarPorClientId([Required] string clientId)
        {
            var response = await _assinaturaService.BuscarAssinaturaTopicoAsync(clientId);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        /// <summary>
        /// Atualizar assinatura topico
        /// </summary>
        /// <param name="request">Objeto de atualização assinatura topico</param>
        /// <returns></returns>
        /// <response code="201">Assinatura atualizada com suceso</response>
        /// <response code="400">Nao foi possivel atualizar assinatura</response>    
        /// <response code="500">Erro interno na api</response>   
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarAssinaturaTopico([FromBody][Required] AtualizarAssinaturaTopicoRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Atualização com erro: {request}", JsonConvert.SerializeObject(request));
                return BadRequest(ModelState);
            }

            var teste = await _assinaturaService.AtualizarAssinaturaTopicoAsync(request);
            return Ok(teste);
        }

        /// <summary>
        /// Consultar assinatura topico por CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns>Assinatura encontrada via CNPJ</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorCNPJ/{cnpj}")]
        [ProducesResponseType(typeof(IEnumerable<AssinaturaTopico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssinaturaTopico>>> BuscarPorCNPJ([Required] string cnpj)
        {
            var response = await _assinaturaService.BuscarAssinaturaTopicoPorCnpjAsync(cnpj);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        /// <summary>
        /// Consultar assinatura topico por Id Consumer
        /// </summary>
        /// <param name="idConsumer">IdConsumer</param>
        /// <returns>Assinatura encontrada via IdConsumer</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorIdConsumer/{idConsumer}")]
        [ProducesResponseType(typeof(IEnumerable<AssinaturaTopico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssinaturaTopico>>> BuscarPorIdConsumer([Required] string idConsumer)
        {
            var response = await _assinaturaService.BuscarAssinaturaTopicoPorIdConsumerAsync(idConsumer);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        /// <summary>
        /// Consultar assinatura topico por Id
        /// </summary>
        /// <param name="id">Id/param>
        /// <returns>Assinatura encontrada via Id</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<AssinaturaTopico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AssinaturaTopico>> BuscarPorId([Required] string id)
        {
            var response = await _assinaturaService.BuscarAssinaturaTopicoPorIdAsync(id);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        /// <summary>
        /// Consultar assinatura topico por Topico
        /// </summary>
        /// <param name="topico">Topico</param>
        /// <returns>Assinatura encontrada via Topico</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorTopico/{topico}")]
        [ProducesResponseType(typeof(IEnumerable<AssinaturaTopico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssinaturaTopico>>> BuscarPorTopico([Required] string topico)
        {
            var response = await _assinaturaService.BuscarAssinaturaPorTopicoAsync(topico);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
