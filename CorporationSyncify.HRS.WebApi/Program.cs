using CorporationSyncify.HRS.WebApi.Authentification;
using CorporationSyncify.HRS.WebApi.Helpers;
using CorporationSyncify.HRS.WebApi.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddIdentityServerAuthentication(builder.Configuration);
builder.Services
    .AddDomain()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.PrepareDataPopulation();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
