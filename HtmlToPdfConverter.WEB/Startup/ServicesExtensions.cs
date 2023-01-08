using Hangfire;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps;
using HtmlToPdfConverter.BL.Services;
using Infra.EasySaga;
using Minio;
using StackExchange.Redis;

namespace HtmlToPdfConverter.WEB.Startup
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
        {
            // By connecting here we are making sure that our service
            // cannot start until redis is ready. This might slow down startup,
            // but given that there is a delay on resolving the ip address
            // and then creating the connection it seems reasonable to move
            // that cost to startup instead of having the first request pay the
            // penalty.
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var configuration = ConfigurationOptions.Parse(connectionString, true);

                configuration.ConnectRetry = 5; // Can be moved to config.
                configuration.ReconnectRetryPolicy = new LinearRetry(5 * 1000); // Can be moved to config.

                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddSingleton(lazyConnection.Value);

            return services;
        }

        public static IServiceCollection AddMinio(this IServiceCollection services, string endpoint, string accessKey,
            string secretKey)
        {
            var minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL()
                .WithReconnectRetryPolicy(10, TimeSpan.FromSeconds(10)) // Can be moved to config.
                .Build();

            services.AddSingleton(minioClient);
            return services;
        }

        public static IServiceCollection AddHangfire(this IServiceCollection services, string msSqlConnectionString)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(msSqlConnectionString));
            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddUploadSourceFileSaga(this IServiceCollection services)
        {
            services.AddTransient<CreateSessionStep>();
            services.AddTransient<FileUploadStep>();
            services.AddTransient<CreateConvertJobStep>();

            services.AddTransient<ISagaScenario<UploadSourceFileContext>>(sp =>
                new SagaBuilder<UploadSourceFileContext>()
                .AddTransaction(sp.GetRequiredService<CreateSessionStep>())
                .AddTransaction(sp.GetRequiredService<FileUploadStep>())
                .AddTransaction(sp.GetRequiredService<CreateConvertJobStep>()));

            return services;
        }

        public static IServiceCollection AddConvertToPdfSaga(this IServiceCollection services)
        {
            services.AddSingleton<IHtmlConverter, PuppeteerHtmlConverter>();

            services.AddTransient<ConvertToPdfStep>();
            services.AddTransient<UploadConvertedFileStep>();
            services.AddTransient<FinishSessionStep>();

            services.AddTransient<ISagaScenario<ConvertToPdfContext>>(sp => 
                new SagaBuilder<ConvertToPdfContext>()
                .AddTransaction(sp.GetRequiredService<ConvertToPdfStep>())
                .AddTransaction(sp.GetRequiredService<UploadConvertedFileStep>())
                .AddTransaction(sp.GetRequiredService<FinishSessionStep>()));

            return services;
        }
    }
}
