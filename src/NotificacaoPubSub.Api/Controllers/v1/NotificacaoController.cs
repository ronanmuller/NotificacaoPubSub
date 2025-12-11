using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificacaoPubSub.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly ILogger<NotificacaoController> _logger;
        private readonly INotificacaoService _notificacaoService;

        public NotificacaoController(
            ILogger<NotificacaoController> logger,
            INotificacaoService notificacaoService)
        {
            _logger = logger;
            _notificacaoService = notificacaoService;
        }

        /// <summary>
        /// Enviar evento
        /// </summary>
        /// <param name="request">Objeto de evento</param>
        /// <returns></returns>
        /// <response code="200">Evento enviado com suceso</response>
        /// <response code="400">Nao foi possivel enviar evento</response>    
        /// <response code="500">Erro interno na api</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<MensagemResponse>> EnviarEvento([FromBody][Required] MensagemNotificacao request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Inserção com erro: {request}", JsonSerializer.Serialize(request));
                //return BadRequest(ModelState);
            }

            return await _notificacaoService.EnviarEventoAsync(request);
            
        }
    }
}
