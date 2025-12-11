using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotificacaoPubSub.Domain.Dtos
{
    public class AtualizarAssinaturaTopicoRequest
    {
        [Required]
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Topico { get; set; }
        public string IdConsumer { get; set; }
        public string UrlCallback { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public bool Ativo { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
    }
}
