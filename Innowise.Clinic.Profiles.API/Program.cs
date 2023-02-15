using Innowise.Clinic.Profiles.AppConfiguration;
using Innowise.Clinic.Profiles.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureSwagger();
builder.Services.AddDbContext<ProfilesDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.ConfigureProfileServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.PrepareDb();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();