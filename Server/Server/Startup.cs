using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;

            string sqlUsername = config["Database:Username"];
            string sqlPwdSecret = config["Database:PasswordKvSecretName"];
            string KvUri = config["KvUri"];

            if (sqlUsername == null || sqlPwdSecret == null || KvUri == null)
            {
                throw new Exception("failed to read KvUri or/and Username or/and PasswordKvSecretName from config");
            }
            var client = new SecretClient(new Uri(KvUri), new DefaultAzureCredential());
            string sqlPassword = client.GetSecret(sqlPwdSecret).Value.Value;
            JmvgDbContext.Initialize(sqlUsername, sqlPassword);
        }

        public IConfiguration Configuration { get; }

        readonly string CORSPolicyAllowSpecificOrigin = "CORSPolicyAllowSpecificOrigin";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CORSPolicyAllowSpecificOrigin,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000");
                                  });
            });
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CORSPolicyAllowSpecificOrigin);

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
