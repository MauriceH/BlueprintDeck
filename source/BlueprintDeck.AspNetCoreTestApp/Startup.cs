using System;
using BlueprintDeck.DependencyInjection;
using BlueprintDeck.ValueSerializer.Serializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BlueprintDeck.AspNetCoreTestApp
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
            services.AddBlueprintDeck(builder =>
            {
                builder.RegisterSerializer<NullableTimeSpanValueSerializer,TimeSpan?>();
                builder.RegisterSerializer<TimeSpanValueSerializer,TimeSpan>();
                builder.RegisterSerializer<DoubleValueSerializer,double>();
                builder.RegisterSerializer<Int32ValueSerializer,int>();
            });
            services.AddSingleton<BlueprintInstance>();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "BlueprintDeck.AspNetCoreTestApp", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlueprintDeck.AspNetCoreTestApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}