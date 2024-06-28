using CFDocumentIndexer.Common.DocumentIndexers;
using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            var connectionString= "Data Source=D:\\Test\\DocumentIndex\\DocumentData.db";

            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.RegisterAllTypes<IDocumentIndexer>(new[] { typeof(TextFileIndexer).Assembly });
                    services.AddTransient<IDocumentFilterManager, DocumentFilterManager>();
                    services.AddTransient<IDocumentIndexManager, DocumentIndexManager>();
                    services.AddTransient<IIndexedDocumentService>((scope) =>
                    {
                        return new SQLiteIndexedDocumentService(connectionString);
                    });

                    services.AddTransient<MainForm>();
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