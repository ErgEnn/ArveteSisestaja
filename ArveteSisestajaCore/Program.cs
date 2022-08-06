using Autofac;
using Autofac.Core;
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
            var container = builder.Build();
            ApplicationConfiguration.Initialize();
            Application.Run(container.Resolve<MainForm>());
        }
    }
}