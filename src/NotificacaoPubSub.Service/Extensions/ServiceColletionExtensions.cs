using Microsoft.Extensions.DependencyInjection;
using NotificacaoPubSub.Data.Repositories;
using NotificacaoPubSub.Domain.Constants;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models.Configuracao;
using NotificacaoPubSub.Domain.Profiles;
using NotificacaoPubSub.Service.Services;

namespace NotificacaoPubSub.Service.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var configuracaoRepository = new DynamoBaseRepository<ConfiguracaoSistema>();
            var configuracaoSistema = configuracaoRepository.BuscarAsync(Chave.CHAVE_CONFIGURACAO).Result;
            services.AddSingleton(configuracaoSistema);

            //Services
            services.AddSingleton<INotificacaoService, NotificacaoService>();
            services.AddSingleton<IAssinaturaTopicoService, AssinaturaTopicoService>();
            services.AddSingleton<ITopicoService, TopicoService>();
            services.AddSingleton<ISqsService, SqsService>();

            //Repository
            services.AddSingleton(typeof(IDynamoBaseRepository<>), typeof(DynamoBaseRepository<>));

            //Mapper
            services.AddAutoMapper(typeof(AssinaturaTopicoProfile).Assembly);
            services.AddAutoMapper(typeof(TopicoProfile).Assembly);

            services.AddHealthChecks();

            return services;
        }
    }
}
