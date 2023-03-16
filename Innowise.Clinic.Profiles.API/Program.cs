using Innowise.Clinic.Profiles.Configuration;
using Innowise.Clinic.Profiles.RequestPipeline;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureProfileRepositories(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.ConfigureProfileServices(builder.Configuration);
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

app.Run();