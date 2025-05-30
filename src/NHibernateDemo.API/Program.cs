using NHibernateDemo.API.Endpoints;
using NHibernateDemo.API.Extensions;
using NHibernateDemo.API.Middleware;
using NHibernateDemo.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNHibernate(builder.Configuration);
builder.Services.AddHandlers();
builder.Services.AddRepositories();

builder.Services.Configure<BasicAuthOptions>(builder.Configuration.GetSection(BasicAuthOptions.BasicAuth));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<BasicAuthMiddleware>();

app.UseHttpsRedirection();

app.MapStudentsRoute();

app.Run();
