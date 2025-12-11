using Amazon.DynamoDBv2.DataModel;

namespace NotificacaoPubSub.Domain.Models
{
    [DynamoDBTable("notificacao-pub-sub")]
    public class AssinaturaTopico
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; }
        [DynamoDBProperty("clientId")]
        public string ClientId { get; set; }
        [DynamoDBProperty("topico")]
        public string Topico { get; set; }
        [DynamoDBProperty("idConsumer")]
        public string IdConsumer { get; set; }
        [DynamoDBProperty("urlCallback")]
        public string UrlCallback { get; set; }
        [DynamoDBProperty("token")]
        public string Token { get; set; }
        [DynamoDBProperty("ativo")]
        public bool Ativo { get; set; }
        [DynamoDBProperty("cnpj")]
        public string CNPJ { get; set; }
        [DynamoDBProperty("email")]
        public string Email { get; set; }
    }
}
