var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("MySQL")!;
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 32)));
});

builder.Services.AddScoped<IRssRepository, RssRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RSS Web-API with POST", Version = "v1.0" });

    c.SwaggerDoc("v2", new OpenApiInfo { Title = "RSS Web-API with GET", Version = "v2.0" });

    c.DocInclusionPredicate((version, desc) =>
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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"RSS Web-API with POST v1.0");
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"RSS Web-API with GET v2.0");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
