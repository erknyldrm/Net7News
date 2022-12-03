using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(policy =>
    {
        policy.Expire(TimeSpan.FromSeconds(5));
    });

    options.AddPolicy("Custom", policy =>
    {
        policy.Expire(TimeSpan.FromSeconds(10));
    });
});

var app = builder.Build();

app.UseOutputCache();

//app.MapGet("/", () =>
//{
//    return Results.Ok(DateTime.UtcNow);
//}).CacheOutput();


app.MapGet("/", [OutputCache()] () =>
{
    return Results.Ok(DateTime.UtcNow);
}).CacheOutput();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


