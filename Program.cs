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

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddCustomAuthentication();

builder.Services.AddCustomSwaggerDocumentation();

var app = builder.Build();

app.UseCustomSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
