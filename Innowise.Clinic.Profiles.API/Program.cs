using Innowise.Clinic.Profiles.AppConfiguration;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.RequestPipeline;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureSwagger();
builder.Services.AddDbContext<ProfilesDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.ConfigureProfileServices();
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
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