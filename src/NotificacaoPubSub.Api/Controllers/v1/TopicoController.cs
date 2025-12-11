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
    public class TopicoController : ControllerBase
    {
        private readonly ILogger<TopicoController> _logger;
        private readonly ITopicoService _topicoService;

        public TopicoController(
            ILogger<TopicoController> logger,
            ITopicoService topicoService)
        {
            _logger = logger;
            _topicoService = topicoService;
        }


        /// <summary>
        /// Consultar todos topico
        /// </summary>
        /// <param></param>
        /// <returns>Topicos encontrados</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Topico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Topico>>> BuscarTodosTopicos()
        {

            _logger.LogInformation("inicianado os testes do log");
            var response = await _topicoService.BuscarTodosTopicosAsync();
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        /// <summary>
        /// Cadastrar topico
        /// </summary>
        /// <param name="request">Objeto de cadastro de topico</param>
        /// <returns></returns>
        /// <response code="201">Topico cadastrado com suceso</response>
        /// <response code="400">Nao foi possivel cadastrar topico</response>    
        /// <response code="500">Erro interno na api</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CadastrarTopico([FromBody][Required] CadastrarTopicoRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Inserção com erro: {request}", JsonConvert.SerializeObject(request));
                return BadRequest(ModelState);
            }

            await _topicoService.CadastrarTopicoAsync(request);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Descadastrar topico
        /// </summary>
        /// <param name="id">Id para descadastro o topico</param>
        /// <returns></returns>
        /// <response code="204">Topico descadastrada com suceso</response>
        /// <response code="400">Nao foi possível descadastrar o topico</response>    
        /// <response code="500">Erro interno na api</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DescadastrarTopico([Required] string id)
        {
            await _topicoService.DescadastrarTopicoAsync(id);
            return NoContent();
        }


        /// <summary>
        /// Atualizar topico
        /// </summary>
        /// <param name="request">Objeto de atualização de topico</param>
        /// <returns></returns>
        /// <response code="201">Topico atualizado com suceso</response>
        /// <response code="400">Nao foi possivel atualizar o topico</response>    
        /// <response code="500">Erro interno na api</response>   
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarTopico([FromBody][Required] AtualizarTopicoRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Atualização com erro: {request}", JsonConvert.SerializeObject(request));
                return BadRequest(ModelState);
            }

            await _topicoService.AtualizarTopicoAsync(request);
            return Ok();
        }

        /// <summary>
        /// Consultar topico por descrição
        /// </summary>
        /// <param name="descricao">Descricao</param>
        /// <returns>Topico encontrado via descricao</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorDescricao/{descricao}")]
        [ProducesResponseType(typeof(IEnumerable<Topico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Topico>>> BuscarPorDescricao([Required] string descricao)
        {
            var response = await _topicoService.BuscarTopicoPorDescricaoAsync(descricao);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        /// <summary>
        /// Consultar topico por id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Topico encontrado via Id</returns>
        /// <response code="200">Retorna item encontrado</response>
        /// <response code="204">Caso nao encontre nenhum item</response>    
        /// <response code="500">Erro interno na api</response>    
        [HttpGet("BuscarPorId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Topico>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Topico>>> BuscarPorId([Required] string id)
        {
            var response = await _topicoService.BuscarTopicoPorIdAsync(id);
            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
