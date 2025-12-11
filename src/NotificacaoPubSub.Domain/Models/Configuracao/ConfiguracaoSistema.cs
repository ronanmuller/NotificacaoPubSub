using Amazon.DynamoDBv2.DataModel;

namespace NotificacaoPubSub.Domain.Models.Configuracao
{
    [DynamoDBTable("Master-Configuracoes-Sistema")]
    public class ConfiguracaoSistema
    {
        [DynamoDBHashKey("sistema")]
        public string Sistema { get; set; }
        [DynamoDBProperty("ConfigTI")]
        public ConfiguracaoTI ConfiguracaoTI { get; set; }
    }
}
