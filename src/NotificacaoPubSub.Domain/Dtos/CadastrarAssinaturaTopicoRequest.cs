using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace NotificacaoPubSub.Domain.Dtos
{
    public class CadastrarAssinaturaTopicoRequest
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string Topico { get; set; }
        [Required]
        public string IdConsumer { get; set; }
        [Required]
        public string UrlCallback { get; set; }
        public string Token { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
    }
}
