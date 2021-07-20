using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestBankAPI.Models;
using Microsoft.OpenApi.Models;
using TestBankAPI.Interfaces;
using TestBankAPI.Implementations;
using System;
using System.Reflection;
using System.IO;
using TestBankAPI.Helper;

namespace TestBankAPI
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
            //repositories
            services.AddScoped<IAccount, AccountsRepository>();
            services.AddScoped<ITransactions, TransactionsRepository>();
            services.AddControllers();
            services.AddDbContext<BankContext>(options =>
            options.UseSqlServer(
            Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60)), (ServiceLifetime.Transient));
            //services.AddDbContext<BankContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalSQLServer")));
            services.AddSwaggerGen();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BankContext db )
        {
            app.UseCors(options=>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            Seed.SeedData(db);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "MyAPI V1");
            });
        }
    }
}
