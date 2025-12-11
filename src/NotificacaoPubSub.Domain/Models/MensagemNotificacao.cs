using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotificacaoPubSub.Domain.Models
{
    public class MensagemNotificacao
    {
        [JsonIgnore]
        public string Id { get; set; }
        [Required]
        public string IdConsumer { get; set; }
        [Required]
        public string Topico { get; set; }
        [Required]
        public object PayloadJson { get; set; }
        [JsonIgnore]
        public string Payload { get; set; }
        [JsonIgnore]
        public AssinaturaTopico AssinaturaTopico { get; set; }
    }
}
