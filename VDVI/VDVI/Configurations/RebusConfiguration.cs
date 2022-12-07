using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;
using Rebus.Transport.InMem;
using System;
using VDVI.Services.MediatR.Models;

namespace VDVI.Client.Configurations
{
    public static class RebusConfiguration
    {
        public static IServiceCollection AddRebus(this IServiceCollection services, IConfiguration configuration)
        {
            // Register handlers
            services.AutoRegisterHandlersFromAssemblyOf<Startup>();


            services.AddRebus((config, provider) => config
                 .Logging(l => l.Use(new MSLoggerFactoryAdapter(provider.GetService<ILoggerFactory>())))
                 .Serialization(s =>
                 {
                     s.UseNewtonsoftJson();
                 })
                 .Transport(t =>
                 {
                     var network = new InMemNetwork(new MSLoggerFactoryAdapter(provider.GetService<ILoggerFactory>()));
                     t.UseInMemoryTransport(network, "VDVI");
                 })
                 .Subscriptions(s => s.StoreInMemory())
                 .Routing(r => r.AddRoutes())
                 .Options(o => o
                     .LogPipeline(verbose: true)
                 )
             );



            return services;
        }

        public static IApplicationBuilder UseRebus(this IApplicationBuilder app, Action<IBus> subscriptions)
        {
            // Register subscriptions
            var bus = app.ApplicationServices.GetService<IBus>();
            subscriptions(bus);


            return app;
        }

        private static void AddRoutes(this StandardConfigurer<IRouter> router)
        {
            router.TypeBased()
                .Map<ApmaSchedulerEvent>("VDVI");
                //.Map<MultiNotification>("VDVI");
        }

        

        public static void AddSubscriptions(this IBus bus)
        {
            bus.Subscribe<ApmaSchedulerEvent>();
        }


    }
}
