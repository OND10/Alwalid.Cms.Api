using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.FileProviders;
using Alwalid.Cms.Api.Settings;
using Alwalid.Cms.Api.Middleware;
using ProductAPI.VSA.Features.Gemini.Endpoints;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureRequestPipeline(app.Environment);

app.Run();