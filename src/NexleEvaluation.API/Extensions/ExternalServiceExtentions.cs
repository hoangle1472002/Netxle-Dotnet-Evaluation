using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using NexleEvaluation.Application.Settings;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NexleEvaluation.API.Extensions
{
    public static class ExternalServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination");
                });
            });

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        public static void ConfigureSwagger(this IServiceCollection services, SwaggerDocOptions swaggerDocOptions)
        {
            services.AddSwaggerGen();
            services.AddOptions<SwaggerGenOptions>()
                .Configure<IApiVersionDescriptionProvider>((swagger, service) =>
                {
                    foreach (ApiVersionDescription description in service.ApiVersionDescriptions)
                    {
                        swagger.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Title = swaggerDocOptions.Title,
                            Version = description.ApiVersion.ToString(),
                            Description = swaggerDocOptions.Description,
                            Contact = new OpenApiContact
                            {
                                Name = swaggerDocOptions.Organization,
                                Email = swaggerDocOptions.Email
                            }
                        });
                    }

                    swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "JWT Authorization header using the Bearer scheme.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });

                    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Id = JwtBearerDefaults.AuthenticationScheme,
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                });
        }
    }
}
