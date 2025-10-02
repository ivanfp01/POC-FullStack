using Application.Registrations;
using AutoMapper;
using Core.Application;
using Filters;
using FluentValidation;
using Infrastructure.Registrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);
            services.AddAutomovilesSqlServer(Configuration);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Automóviles API", Version = "v1" });
                
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            services.AddHealthChecks();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Title = "Error de validación",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Uno o más errores de validación ocurrieron."
                    };

                    return new BadRequestObjectResult(problemDetails);
                };
            });

            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.Add<BaseExceptionFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Automóviles API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            if (Configuration.GetValue<bool>("EventBus:Enabled", false))
            {
                CustomMapper.Instance = app.ApplicationServices.GetRequiredService<IMapper>();
            }

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            
            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            
            if (Configuration.GetValue<bool>("EventBus:Enabled", false))
            {
                UseEventBus(app);
            }
            
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private void UseEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }
    }
}
