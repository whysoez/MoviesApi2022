using Microsoft.EntityFrameworkCore;
using MoviesApi2022.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// add cors
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddDbContext<MoviesDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MovieConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("error");
}

app.UseHttpsRedirection();

// config cors allow anyway
app.UseCors(option =>
{
    option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});


app.UseAuthentication();
app.MapControllers();

app.UseAuthorization();

app.Run();
