using System.ComponentModel.DataAnnotations;

namespace NotificacaoPubSub.Domain.Dtos
{
    public class CadastrarTopicoRequest
    {
        [Required]
        public string Descricao { get; set; }
    }
}
