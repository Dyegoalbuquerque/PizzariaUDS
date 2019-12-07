using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Repositorys.Concrete;
using Webapi.Domain.Services.Abstract;
using Webapi.Domain.Services.Concrete;

namespace Webapi
{
    public class Startup
    {
        readonly string corsUDS = "_corsUDS";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IIngredienteRepository, IngredienteRepository>();
            services.AddTransient<IItemAdicionalRepository, ItemAdicionalRepository>();
            services.AddTransient<IItemPrecoRepository, ItemPrecoRepository>();
            services.AddTransient<IModoPreparoRepository, ModoPreparoRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
            services.AddTransient<ISaborRepository, SaborRepository>();
            services.AddTransient<IModoPreparoIngredienteRepository, ModoPreparoIngredienteRepository>();
            services.AddTransient<IItemAdicionalService, ItemAdicionalService>();
            services.AddTransient<IItemPrecoService, ItemPrecoService>();
            services.AddTransient<IModoPreparoService, ModoPreparoService>();
            services.AddTransient<IPedidoService, PedidoService>();
            services.AddTransient<ISaborService, SaborService>();           

            services.AddCors(options =>
            {   
                options.AddPolicy(corsUDS,
                builder =>
                {
                     builder.AllowAnyOrigin() 
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                });               
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "UDS API", Version = "v1" });               
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UDS API V1");
            });
            app.UseCors(corsUDS);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
