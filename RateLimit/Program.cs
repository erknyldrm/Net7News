using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Algorythms
#region Fixed Window
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(12);
        limiter.PermitLimit = 4;
        limiter.QueueLimit = 2;
        limiter.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});
#endregion

#region Sliding 
// Fixed wind alogoritmasına benzer, her sabit sürede bir zaman aralığnda istekleri sınırlandırmakta lakin sürenin yarısından sonra
// diğer periyodun istek kotasını harcayacak şekilde istekleri karşılar.
builder.Services.AddRateLimiter(options =>
{
    options.AddSlidingWindowLimiter("Sliding", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(12);
        limiter.PermitLimit = 4;
        limiter.QueueLimit = 2;
        limiter.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        limiter.SegmentsPerWindow = 2;
    });
});
#endregion

#region Token Bucket
// Her bir periyodda işlenecek istek sayısı kadar token üretilmekte, eğer bu tokenlar kullanıldıysa diğer periyodda borç alınabilir.
// Her periyodda token miktarı kadar token üretilecek ve bu şekilde rate limit kullanılacaktır. 
// Her periyodun max token limiti verilen sabit sayı kadar olacaktır.
builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("Token", limiter =>
    {
        limiter.TokenLimit = 4;
        limiter.TokensPerPeriod = 4;
        limiter.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = 2;
        limiter.ReplenishmentPeriod = TimeSpan.FromSeconds(12);
    });
});
#endregion

#region Concurrency
// Asenkron istekleri sınırlandırmak için kullanılır, Her istek concurrency sınırınI bir azaltmakta ve bittikleri takdirde bu sınırı bir artırmaktadırlar. 
// Diğer algoritmalara göre sadece asenkron istekleri sınırlandırır.
builder.Services.AddRateLimiter(options =>
{
    options.AddConcurrencyLimiter("Concurrency", limiter =>
    {
        limiter.PermitLimit = 4;
        limiter.QueueLimit = 2;
        limiter.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});
#endregion
#endregion

#region Attributes
#region EnableRateLimiting
//Controller ya da action seviyesinde istenilen politikadaki rate limtii devreye almaya yarar.
#endregion
#region DisableRateLimiting
//Controller seviyesinde devreye alınmış bir rate limit politikasını action seviyesinde pasifleştirmeyi sağlar. 
#endregion
#endregion

#region Minimal Api

//app.MapGet("/", () =>
//{

//}).RequireRateLimiting("PolicyName");
#endregion

#region OnRejected Property
//Rate limit uygulanan operasyonlarda sınırdan dolayı boşa çıkan isteklerin söz konusu olduğu durumlarda loglama vs. gibi işlemleri
// yapabilmek için kullanılan event mantığında bir property dir.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", limiter =>
    {
        limiter.Window = TimeSpan.FromSeconds(12);
        limiter.PermitLimit = 4;
        limiter.QueueLimit = 2;
        limiter.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    options.OnRejected = (context, cancellationToken) =>
    {
        //Log
        return new();
    };
});
#endregion

#region Custom rate limit policy
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string, CustomRateLimitPolicy>("CustomPolicy");
});
#endregion

var app = builder.Build();

//app.MapGet("/", () =>
//{

//}).RequireRateLimiting("PolicyName");

app.UseRateLimiter();
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

class CustomRateLimitPolicy : IRateLimiterPolicy<string>
{
    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => (context, cancellationToken) =>
    {
        return new();
    };

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.GetFixedWindowLimiter("", _=> new()
        {
            PermitLimit = 4,
            Window = TimeSpan.FromSeconds(12),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 2
        });
    }
}

