using Innowise.Clinic.Profiles.Configuration;
using Innowise.Clinic.Profiles.RequestPipeline;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureProfileRepositories(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.ConfigureProfileServices(builder.Configuration);
builder.Services.ConfigureFilters();
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

await app.PrepareDb();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("The Profiles service is starting");
app.Run();
Log.Information("The Profiles service is stopping");
await Log.CloseAndFlushAsync();