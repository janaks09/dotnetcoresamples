using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using SwaggerSample.Infrastructure.Swagger;

namespace SwaggerSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Environment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string swaggerCommentXmlPath = string.Empty;
            if (Environment.IsDevelopment()) //while development
                swaggerCommentXmlPath = $@"{Environment.ContentRootPath}\bin\Debug\netcoreapp1.0\SwaggerSample.xml";
            else //while production
                swaggerCommentXmlPath = $@"{Environment.ContentRootPath}\SwaggerSample.xml";


            // Add framework services.
            services.AddMvc();

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "My awesome API",
                    Description = "My Awesome API by @janaks09",
                    TermsOfService = "NA",
                    Contact = new Contact() { Name="Your name", Email="your email", Url="your url" }
                });

                options.IncludeXmlComments(swaggerCommentXmlPath); //Includes XML comment file
                options.OperationFilter<FileOperation>();
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
