using System;
using StructureMap;

namespace MyCompiler.IoC
{
    public static class DefaultConfig
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Config(Type program)
        {
            // add the framework services
            //var services = new ServiceCollection()
            //    .AddLogging();

            // add StructureMap
            var container = new Container();
            container.Configure(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(program);
                    _.WithDefaultConventions();
                });
                // Populate the container using the service collection
                //config.Populate(services);
            });

            ServiceProvider = container.GetInstance<IServiceProvider>();

            // rest of method as before
        }
    }
}
