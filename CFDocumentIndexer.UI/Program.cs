using CFDocumentIndexer.Indexers;
using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Microsoft;
using CFDocumentIndexer.Microsoft.Indexers.Images;
using CFDocumentIndexer.Microsoft.Interfaces;
using CFDocumentIndexer.Microsoft.Models;
using CFDocumentIndexer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Reflection;

namespace CFDocumentIndexer.UI
{
    internal static class Program
    {       
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Create a host builder to build the service provider
        /// </summary>
        /// <returns></returns>
        static IHostBuilder CreateHostBuilder()
        {
            // TODO: Move to config
            //var connectionString= "Data Source=D:\\Test\\DocumentIndex\\DocumentData.db";            

            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    // Register document indexers in Common assembly
                    services.RegisterAllTypes<IDocumentIndexer>(new[] { typeof(TextFileIndexer).Assembly });
                    
                    services.AddTransient<IDocumentFilterManager, DocumentFilterManager>();
                    services.AddTransient<IDocumentIndexManager, DocumentIndexManager>();
                    
                    services.AddDocumentIndexerMicrosoft();
               
                    services.AddTransient<IIndexedDocumentService>((scope) =>
                    {
                        var connectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"].ToString();
                        return new SQLiteIndexedDocumentService(connectionString);
                    });
                    
                    services.AddTransient<MainForm>();
                });
        }        

        /// <summary>
        /// Adds document indexer for Microsoft
        /// </summary>
        /// <param name="services"></param>
        private static void AddDocumentIndexerMicrosoft(this IServiceCollection services)
        {
            services.RegisterAllTypes<IDocumentIndexer>(new[] { typeof(VisionImageFileIndexer).Assembly });

            services.AddSingleton<IVisionConfig>((scope) =>
            {
                return new VisionConfig()
                {
                    APIKey = ConfigurationManager.AppSettings["VisionAPIKey"].ToString(),
                    Endpoint = ConfigurationManager.AppSettings["VisionEndpoint"].ToString()
                };
            });
        }

        /// <summary>
        /// Registers all types implementing interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <param name="lifetime"></param>
        private static void RegisterAllTypes<T>(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
            {
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
            }
        }
    }
}