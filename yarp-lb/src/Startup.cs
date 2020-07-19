using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ReverseProxy.Middleware;
using Microsoft.ReverseProxy.RuntimeModel;

namespace YarpProxy
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddReverseProxy()
                .LoadFromConfig(Configuration.GetSection("ReverseProxy"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy(proxyPipeline =>
                {
                    var logger = proxyPipeline.ApplicationServices.GetService<ILogger<Startup>>();
                    logger.LogInformation("Entry pipeline...");

                    proxyPipeline.Use(async (context, next) =>
                    {

                        IPHostEntry host = await Dns.GetHostEntryAsync("app");

                        logger.LogInformation(@$"GetHostEntry({host.HostName}) returns
    alias: {string.Join(",", host.Aliases)}
    addresses: {string.Join(",", host.AddressList.Select(x => x.MapToIPv4().ToString()))}");

                        var hostAddresses = await Dns.GetHostAddressesAsync("app");

                        logger.LogInformation($"{nameof(Dns.GetHostAddressesAsync)}: {string.Join(",", hostAddresses.Select(x => x.MapToIPv4().ToString()))}");


                        var availableDestinationsFeature = context.Features.Get<IAvailableDestinationsFeature>();

                        logger.LogInformation("Current Destinations: {Destinations}", availableDestinationsFeature.Destinations);

                        availableDestinationsFeature.Destinations = hostAddresses.Select(x => new DestinationInfo("foo")
                        {
                            Config = {Value = new DestinationConfig($"http://{x.MapToIPv4()}")},
                        }).ToList();

                        logger.LogInformation("New Destinations: {Destinations}", availableDestinationsFeature.Destinations);

                        await next();

                    });



                    proxyPipeline.UseAffinitizedDestinationLookup();
                    proxyPipeline.UseProxyLoadBalancing();
                    proxyPipeline.UseRequestAffinitizer();
                    logger.LogInformation("Exit pipeline...");

                });

                endpoints.MapGet("/hello", context => context.Response.WriteAsync("Hello World!"));
            });

            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            // app.UseRouting();

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapGet("/", async context =>
            //     {
            //         await context.Response.WriteAsync("Hello World!");
            //     });
            // });
        }
    }
}
