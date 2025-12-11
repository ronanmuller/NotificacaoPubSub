using System.ComponentModel.DataAnnotations;

namespace NotificacaoPubSub.Domain.Dtos
{
    public class AtualizarTopicoRequest
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Descricao { get; set; }
    }
}
