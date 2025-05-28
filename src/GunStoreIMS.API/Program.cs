using System.Text.Json.Serialization;
using FluentValidation;                            // ← for AddValidatorsFromAssemblyContaining
using GunStoreIMS.Abstractions.Interfaces;          // ← your service interfaces
using GunStoreIMS.Application.Services;            // ← your service implementations
using GunStoreIMS.Shared.Validation;          // ← your validators (if you put them here)
using GunStoreIMS.Application.Mapping;
using GunStoreIMS.Persistence.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GunStoreIMS.Shared.Dto;             // ← your DTOs
using GunStoreIMS.Domain.Models;          // ← your domain models
using GunStoreIMS.Shared.Enums;           // ← your enums
using GunStoreIMS.Domain.Utilities;       // ← your shared utilities

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using GunStoreIMS.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 0) Serilog as the host logger
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));

// 1) EF Core with pooling
builder.Services.AddDbContextPool<FirearmsInventoryDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Missing DefaultConnection")));

// 2) Application services (your business logic)
// — register each interface → implementation
builder.Services.AddScoped<IAcquisitionRecordRepository, EfAcquisitionRecordRepository>();
builder.Services.AddScoped<IAcquisitionRecordService, AcquisitionRecordService>();
builder.Services.AddScoped<IDealerRecordService, DealerRecordService>();
builder.Services.AddScoped<IDispositionRecordService, DispositionRecordService>();
builder.Services.AddScoped<IFirearmService, FirearmService>();


// 3) AutoMapper
builder.Services.AddAutoMapper(typeof(AdRecordsProfile),
    typeof(FirearmProfile),
    typeof(DealerProfile),
    typeof(CorrectionProfile)
    );




// 5) Configure file‐upload limits
builder.Services.Configure<FormOptions>(opts =>
{
    opts.MultipartBodyLengthLimit = 100 * 1024 * 1024;
    opts.MultipartHeadersLengthLimit = 16 * 1024;
});

// 6) MVC + JSON settings
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 7) Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FIMS API",
        Version = "v1",
        Description = "4473 + bound‐book operations"
    });

    // If you add JWT later, you can also configure your Bearer scheme here
});

// 8) CORS for your React app
const string CorsPolicy = "FimsCorsPolicy";
builder.Services.AddCors(o => o.AddPolicy(CorsPolicy, p =>
    p.WithOrigins("http://localhost:5173")
     .AllowAnyHeader()
     .AllowAnyMethod()
));

// ——————————————————————————————————————————————————————————  
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIMS API v1");
        c.DocumentTitle = "FIMS API Explorer";
    });
    app.MapGet("/", ctx => {
        ctx.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(CorsPolicy);
// app.UseAuthentication();  // once you add JWT
app.UseAuthorization();
app.MapControllers();
app.Run();
