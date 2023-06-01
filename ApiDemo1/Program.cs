using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("1");
    await next();
    await context.Response.WriteAsync("2");
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("3");
    await next();
    await context.Response.WriteAsync("4");
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("5");
});

app.Run();
