using Amazon.DynamoDBv2.DataModel;
using System;

namespace NotificacaoPubSub.Domain.Models
{
    [DynamoDBTable("historico-notificacao-pub-sub")]
    public class HistoricoNotificacao
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; }
        [DynamoDBProperty("notificacaoPubSubId")]
        public string NotificacaoPubSubId { get; set; }
        [DynamoDBProperty("clientId")]
        public string ClientId { get; set; }
        [DynamoDBProperty("topico")]
        public string Topico { get; set; }
        [DynamoDBProperty("idConsumer")]
        public string IdConsumer { get; set; }
        [DynamoDBProperty("payload")]
        public string Payload { get; set; }
        [DynamoDBProperty("dataInclusao")]
        public DateTime DataInclusao { get; set; }
    }
}
