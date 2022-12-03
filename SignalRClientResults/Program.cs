using SignalRClientResults.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(policy => policy.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowCredentials().AllowAnyOrigin().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<MyHub>("/myhub");

app.Run();


#region SignalR - Client Results
//Server, client'a mesah göndermenin dışında client 'tan da return ile bir result isteyebilir.
//Bunun için server' da InvoceAsync ile istemciden bir sonuç döndürmesi beklenir.

#endregion

#region .NET Client

#endregion