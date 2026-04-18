var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
});

var app = builder.Build();

// Configure the HTTP request pipeline.




app.MapGet("/", () => Results.Json(new { message = "API is running" }));

app.MapGet("/health", () => Results.Json(new { message = "healthy" }));

app.MapGet("/me", () => Results.Json(new
{
    name = "Adjerese Precious",
    email = "adjereseprecious9@gmail.com",
    github = "https://github.com/adjereseprecious/personalapi"
}));


app.Run();


