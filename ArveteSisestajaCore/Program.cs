using Autofac;
using Autofac.Core;
using BLL;
using DAL;

namespace ArveteSisestajaCore
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AppDbContext>().AsSelf();
            builder.RegisterType<MainForm>().AsSelf();
            builder.RegisterType<OmnivaHandler>().AsSelf();
            builder.RegisterType<ANCHandler>().AsSelf();
            builder.RegisterType<InvoiceService>().AsSelf();
            var container = builder.Build();
            ApplicationConfiguration.Initialize();
            SettingsHandler.LoadSettings();
            Application.Run(container.Resolve<MainForm>());
        }
    }
}