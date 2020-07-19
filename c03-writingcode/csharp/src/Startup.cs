using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnDockerWebApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace LearnDockerWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LearnDockerDbContext>(options =>
            {
                options.EnableDetailedErrors();
                var userName = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "demo";
                var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "secret";
                var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "demo";
                var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";

                var connectionString = $"Host={dbHost};Database={dbName};Username={userName};Password={password};ConnectionIdleLifetime=5;ConnectionPruningInterval=5;MinPoolSize=2;MaxPoolSize=100";
                options.UseNpgsql(connectionString, npgsqlOpts =>
                {
                    npgsqlOpts.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), new[] {"retry-fails"});
                });
            });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (ctx, next) => {
                    var host = Environment.MachineName;
                    var addressPath = ctx.Request.Path.ToString();
                    var ipAddress = ctx.Connection.LocalIpAddress.MapToIPv4().ToString();
                    var timestamp = DateTimeOffset.Now;

                    var logEntry = new RequestEntry() {
                        Host = host,
                        Ip = ipAddress,
                        Path = addressPath,
                        RequestedAt = timestamp.ToString()
                    };

                    await RecordEntry();

                    async Task RecordEntry()
                    {
                        var dbContext = ctx.RequestServices.GetService<LearnDockerDbContext>();
                        await dbContext.Requests.AddAsync(logEntry);
                        await dbContext.SaveChangesAsync();
                    }

                    await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapGet("/hello",  async ctx => {
                    var dbContext = ctx.RequestServices.GetService<LearnDockerDbContext>();
                    var allEntries = await dbContext.Requests
                        .AsNoTracking()
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                    var jsonContent = System.Text.Json.JsonSerializer.Serialize(allEntries);
                    await ctx.Response.WriteAsync(jsonContent, Encoding.UTF8);
                });
            });
        }
    }
}
