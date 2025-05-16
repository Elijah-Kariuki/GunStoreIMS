using System.Text.Json.Serialization;
using GunStoreIMS.Application.Mapping;
using GunStoreIMS.Persistence.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// — you don’t need to re‑AddJsonFile: CreateBuilder already does that for
//   appsettings.json, appsettings.{ENV}.json, user secrets (in Dev), and env vars.

// 1) EF Core with pooling
builder.Services.AddDbContextPool<FirearmsInventoryDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Missing DefaultConnection")));

// 2) AutoMapper
builder.Services.AddAutoMapper(typeof(Form4473MappingProfile).Assembly);

// 3) Configure file‑upload limits
builder.Services.Configure<FormOptions>(opts =>
{
    opts.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
    opts.MultipartHeadersLengthLimit = 16 * 1024;
});

// 4) MVC + JSON settings
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        // keep your existing choices:
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 5) Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FIMS API",
        Version = "v1",
        Description = "4473 + bound-book operations"
    });
});

// 6) CORS for your React app
const string CorsPolicy = "FimsCorsPolicy";
builder.Services.AddCors(o => o.AddPolicy(CorsPolicy, p =>
    p.WithOrigins("http://localhost:5173")
     .AllowAnyHeader()
     .AllowAnyMethod()
// .AllowCredentials(); // if you need cookies/auth headers
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIMS API v1");
        c.DocumentTitle = "FIMS API Explorer";
    });

    // Redirect root (“/”) to Swagger in Dev
    app.MapGet("/", ctx =>
    {
        ctx.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();

// 1) Routing
app.UseRouting();

// 2) CORS (after routing, before auth)
app.UseCors(CorsPolicy);

// 3) (If you add authentication later) app.UseAuthentication();
app.UseAuthorization();

// 4) Map your controllers
app.MapControllers();

app.Run();
