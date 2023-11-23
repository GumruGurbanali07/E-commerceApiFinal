using Business.Consumer;
using Business.DependencyResolvers;

using Business.Policy;
using DataAccess.DataHelper;
using Entities.ShareModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Run();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<ReceiveEmailConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqps://dgonmxbi:NxJ019hezv0Bkb7pGBzaWtBiBfGULV6V@puffin.rmq2.cloudamqp.com/dgonmxbi");
        cfg.Message<SendEmailCommand>(x => x.SetEntityName("SendEmailCommand"));
        cfg.ReceiveEndpoint("send-email-command", c =>
        {
            c.ConfigureConsumer<ReceiveEmailConsumer>(ctx);
        });
    });
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Web API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});
builder.Services.AddMemoryCache();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    
    .WriteTo.File("logs/myAllLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    DataSeeder.AddData();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


