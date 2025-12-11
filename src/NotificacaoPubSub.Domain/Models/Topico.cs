using Amazon.DynamoDBv2.DataModel;

namespace NotificacaoPubSub.Domain.Models
{
    [DynamoDBTable("notificacao-pub-sub-topico")]
    public class Topico
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; }
        [DynamoDBProperty("descricao")]
        public string Descricao { get; set; }
    }
}
