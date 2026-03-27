using Product.API.Middlewares;
using Product.Application;
using Product.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProductApplication();
builder.Services.AddProductInfrastructure(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Request loglama middleware'i
app.UseMiddleware<RequestLoggingMiddleware>();

// Exception middleware'i
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();