namespace OUTLOUD_Test_proj.Extensions
{
    public static class SwaggerDocumentationExtensions
    {
        public static IServiceCollection AddCustomSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web-API with token auth", Version = "v1.0" });

                options.SwaggerDoc("v2", new OpenApiInfo { Title = "Web-API with bearer token auth", Version = "v2.0" });


                options.DocInclusionPredicate((version, desc) =>
                {
                    var endpointMetadata = desc.ActionDescriptor.EndpointMetadata;

                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                        return false;

                    var specificVersion = endpointMetadata
                            .Where(data => data is MapToApiVersionAttribute)
                            .SelectMany(data => (data as MapToApiVersionAttribute)!.Versions)
                            .Select(apiVersion => apiVersion.ToString())
                            .SingleOrDefault();

                    if (!string.IsNullOrEmpty(specificVersion))
                        return $"v{specificVersion[..^2]}" == version;

                    var versions = endpointMetadata
                            .Where(data => data is ApiVersionAttribute)
                            .SelectMany(data => (data as ApiVersionAttribute)!.Versions)
                            .Select(apiVersion => apiVersion.ToString());

                    return versions.Any(v => $"v{v}.0" == version);
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Authorization Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Web-API with token auth");
                c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"Web-API with bearer token auth");
            });

            return app;
        }
    }
}
