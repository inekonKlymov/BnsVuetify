using Bns.Api;
//using Bns.Api.Auth;
using Bns.Api.Helpers;
using Bns.Api.Middlewares;
using Bns.Application;
using Bns.Domain;
using Bns.Domain.Common.Startup;
using Bns.Domain.Users;
using Bns.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.EnableAnnotations();
    //c.IncludeXmlComments(Assembly.GetExecutingAssembly());
    c.SwaggerDoc("v1", new() { Title = "Bns.Api", Version = "v1" });

    c.EnableAnnotations();
    //c.SwaggerDoc("ClickHouse", new() { Title = "ClickHouse", Version = "v1" });
    //c.DocInclusionPredicate((docName, apiDesc) =>
    //{
    //    if (docName == "v1")
    //    {
    //        // В v1 показываем только те, у кого нет GroupName или GroupName не ClickHouse
    //        return string.IsNullOrEmpty(apiDesc.GroupName) || apiDesc.GroupName != "ClickHouse";
    //    }
    //    if (docName == "ClickHouse")
    //    {
    //        // В ClickHouse показываем только те, у кого GroupName ClickHouse
    //        return apiDesc.GroupName == "ClickHouse";
    //    }
    //    return false;
    //});
});
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // если используете appsettings.json
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();

app.Services.InitInfrastructure();

// Use CORS
app.UseCors(Bns.Api.Authorization.Policies.VuePolicy);
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<SerilogRequestLoggingMiddleware>();
//app.UseMiddleware<JwtMiddleware>();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//    c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bns.Api v1");
//    c.SwaggerEndpoint("/swagger/ClickHouse/swagger.json", "ClickHouse"); // <--- добавить
//});
//}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth/").MapIdentityApi<User>();

app.Run();