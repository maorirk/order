using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderMonitor.Interfaces;
using OrderMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMonitor
{
    public class Bootstrapper
    {
        private readonly ServiceProvider _serviceProvider;
        public Bootstrapper()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            var mainWin = _serviceProvider.GetService<MainWindow>();
            mainWin.Show();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddScoped<Prism.Events.IEventAggregator, Prism.Events.EventAggregator>();
            services.AddTransient<IOrder, OrderMonitorViewModel>();
        }

    }
}
