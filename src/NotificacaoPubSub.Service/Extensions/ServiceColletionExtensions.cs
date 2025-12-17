using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.EventBridge;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.SecretsManager; // <-- ADICIONADOusing Amazon;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using NotificacaoPubSub.Data.Repositories;
using NotificacaoPubSub.Domain.Aws;
using NotificacaoPubSub.Domain.Constants;
using NotificacaoPubSub.Domain.Interfaces.Repositories;
using NotificacaoPubSub.Domain.Interfaces.Services;
using NotificacaoPubSub.Domain.Models.Configuracao;
using NotificacaoPubSub.Domain.Profiles;
using NotificacaoPubSub.Service.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NotificacaoPubSub.Service.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var configuracaoRepository = new DynamoBaseRepository<ConfiguracaoSistema>();
            var configuracaoSistema = configuracaoRepository.BuscarAsync(Chave.CHAVE_CONFIGURACAO).Result;
            services.AddSingleton(configuracaoSistema);

            // Inicialização dos serviços AWS
            ConfigureAwsServices(services);

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

            services.ConfigureOpenTelemetry();
            services.AddHealthChecks();

            return services;
        }

        [ExcludeFromCodeCoverage]
        public static void ConfigureOpenTelemetry(this IServiceCollection services)
        {
            const string serviceName = "notificacaopubsub";
            const string activitySourceName = "notificacaopubsub";

            // Endpoint OTLP (Datadog / OpenTelemetry Collector)
            var otlpEndpoint = new Uri("http://localhost:4318/v1/traces");

            services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                    resource.AddService(serviceName))

                // Tracing
                .WithTracing(tracing =>
                {
                    tracing
                        .AddSource(activitySourceName)                        // Spans customizados
                        .AddAspNetCoreInstrumentation(options =>              // Instrumentação automática ASP.NET Core
                        {
                            options.RecordException = true;

                            // Tags customizadas na requisição
                            options.EnrichWithHttpRequest = (activity, request) =>
                            {
                                activity.SetTag("app.environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                                activity.SetTag("http.method", request.Method);
                                activity.SetTag("http.url", request.Path);
                            };

                            // Tags customizadas na resposta
                            options.EnrichWithHttpResponse = (activity, response) =>
                            {
                                activity.SetTag("http.status_code", response.StatusCode);
                            };

                            // Captura exceptions
                            options.EnrichWithException = (activity, exception) =>
                            {
                                activity.SetTag("error", true);
                                activity.SetTag("error.message", exception.Message);
                            };
                        })
                        .AddHttpClientInstrumentation()                        // Instrumentação automática HttpClient
                        .AddOtlpExporter(options =>                             // Exporta traces para Datadog via OTLP
                        {
                            options.Endpoint = otlpEndpoint;
                        });
                })

                // Logging
                .WithLogging(logging =>
                {
                    logging.AddOtlpExporter(options =>                       // Exporta logs via OTLP
                    {
                        options.Endpoint = otlpEndpoint;
                    });
                });

            // ActivitySource para spans customizados
            services.AddSingleton(new ActivitySource(activitySourceName));
        }

        /// <summary>
        /// Configura AWS SDK (S3, DynamoDB e Secrets Manager).
        /// Se existir configuração em <c>configuracoes</c> tenta ler via reflection os campos "Aws.Region" e "Aws.Profile".
        /// Caso não haja valores, o SDK usará a default credential chain (variáveis de ambiente, ~/.aws/credentials, roles, etc.).
        /// </summary>
        private static void ConfigureAwsServices(IServiceCollection services)
        {
            services.AddDefaultAWSOptions(new AWSOptions());

            // Registra opções padrão e serviços AWS usados pela aplicação
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddAWSService<IAmazonSecretsManager>();
            services.AddAWSService<IAmazonEventBridge>();   // EventBridge
            services.AddAWSService<IAmazonSQS>();           // SQS

            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        }
    }
}
