using BattleShipLiteWPF.View;
using BattleShipLiteWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BattleShipLiteWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<BattleShipViewModel>();
            services.AddTransient<StartViewModel>();
            services.AddTransient<PlayerInnitViewModel>();
            services.AddTransient<FieldViewModel>();
            services.AddTransient<PlaceShipsViewModel>();
            services.AddTransient<PlayerSetupViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
