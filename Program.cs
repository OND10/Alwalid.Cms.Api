using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Alwalid.Cms.Api.Settings;
using Alwalid.Cms.Api.Middleware;
using ProductAPI.VSA.Features.Gemini.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddOData(opt =>
{
    opt.Select().Filter().OrderBy();
}).AddNewtonsoftJson(x=>x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


// Register the GemeinSettings to ease IOtions design pattern
builder.Services.Configure<GeminiSettings>(builder.Configuration.GetSection(nameof(GeminiSettings)));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(
          "http://localhost:5173"
        )
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .WithHeaders("Content-Type")
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Application Services
builder.Services.AddApplication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

new GenerateContentEndpoint().MapEndpoint(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RateLimitingMiddleware>();

app.UseCors("ProductionPolicy");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
