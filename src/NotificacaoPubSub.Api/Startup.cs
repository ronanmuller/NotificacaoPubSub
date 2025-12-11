using DiagnosticsUtil;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using NotificacaoPubSub.Service.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NotificacaoPubSub.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices();

            services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.Name);

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Notificacao Api - Api Notificacao",
                    Version = "v1",
                    Description = "API de Notificacao"
                });

                var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    context.Response.ContentType = "application/json; charset=utf-8";

                    if (ex is ResultException rex)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        var resp = new { rex.CodigoErro, mensagem = rex.Message };
                        logger.LogWarning("Erro conhecido: {Mensagem}", rex.Message);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(resp));
                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    var error = new { context.Response.StatusCode, mensagem = ex?.Message };
                    logger.LogError(ex, "Erro inesperado");

                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                });
            });

            if (env.IsProduction())
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseHealthChecks("/health");
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
