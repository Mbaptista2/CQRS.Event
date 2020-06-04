using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Castle.Facilities.AspNetCore;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CQRS.Domain;
using CQRS.Domain.Bus;
using CQRS.Domain.Cache;
using CQRS.Domain.Commands;
using CQRS.Domain.Events;
using CQRS.Domain.Events.Handlers;
using CQRS.Domain.Interfaces;
using CQRS.Domain.Services;
using CQRS.Infra.Data;
using CQRS.Infra.Data.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CQRS
{
    public class Startup
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private static readonly WindsorContainer Container = new WindsorContainer();
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private void RegisterApplicationComponents(IServiceCollection services)
        {
            // Application components
            Container.Register(
                    Component.For<IEventPublisher>().ImplementedBy<AMQPEventPublisher>().LifeStyle.Singleton,
                    Component.For<AMQPEventSubscriber>().LifeStyle.Singleton,
                    Component.For<ClienteCommandHandler>().LifeStyle.Transient,
                    Component.For<IClienteMongoDbRepository>().ImplementedBy<ClienteMongoDbRepository>().LifeStyle.Singleton,
                    Component.For<IClienteService>().ImplementedBy<ClienteService>().LifeStyle.Transient,
                    Component.For<Domain.Interfaces.ISession>().ImplementedBy<Session>(),
                    Component.For<IEventStore>().ImplementedBy<ClienteEventStore>().LifeStyle.Singleton,
                    Component.For<IBusEventHandler>().ImplementedBy<ClienteCriadoEventHandler>()
                        .Named("CustomerCreatedEventHandler").LifeStyle.Singleton,
                    Component.For<IBusEventHandler>().ImplementedBy<ClienteAtualizadoEventHandler>()
                        .Named("CustomerUpdatedEventHandler").LifeStyle.Singleton,
                    Component.For<IBusEventHandler>().ImplementedBy<ClienteExcluidoEventHandler>()
                        .Named("CustomerDeletedEventHandler").LifeStyle.Singleton,
                    Component.For<IRepository>().UsingFactoryMethod(
                        kernel =>
                        {
                            return new CacheRepository(new Repository( //
                                kernel.Resolve<IEventStore>(), kernel.Resolve<IEventPublisher>()), kernel.Resolve<IEventStore>());
                        }));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup component model contributors for making windsor services available to IServiceProvider
            Container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));

            // Custom application component registrations, ordering is important here
            RegisterApplicationComponents(services);

            services.AddControllers();

            // Castle Windsor integration, controllers, tag helpers and view components, this should always come after RegisterApplicationComponents
             services.AddWindsor(Container,
                opts => opts.UseEntryAssembly(typeof(Controllers.ClienteController).Assembly)); // <- Optional
     
           // new AutofacServiceProvider(ConfigureAutoFac(services));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Container.GetFacility<AspNetCoreFacility>().RegistersMiddlewareInto(app);
          
        }

        public class FrameworkMiddleware : IMiddleware
        {
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                await next(context);
            }
        }
    }
}
