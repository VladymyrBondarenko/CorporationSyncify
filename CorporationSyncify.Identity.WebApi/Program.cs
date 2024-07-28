using CorporationSyncify.Identity.WebApi.Configurations;
using CorporationSyncify.Identity.WebApi.Data;
using CorporationSyncify.Identity.WebApi.Helpers;
using CorporationSyncify.Identity.WebApi.Installers;
using CorporationSyncify.Identity.WebApi.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddIdentityServerAuthentication(builder.Configuration);

var assembly = typeof(Program).Assembly.GetName().Name;
var identityDbConnection = builder.Configuration.GetConnectionString("IdentityDbConnection");

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddDbContext<CorporationSyncifyIdentityDbContext>(options =>
{
    options.UseSqlServer(identityDbConnection,
        opt => opt.MigrationsAssembly(assembly));
});

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CorporationSyncifyIdentityDbContext>();

var config = new IdentityServerConfiguration(builder.Configuration);

builder.Services
    .AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => 
            b.UseSqlServer(
                identityDbConnection, 
                opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(
                identityDbConnection,
                opt => opt.MigrationsAssembly(assembly));
    })
    .AddInMemoryIdentityResources(config.IdentityResources)
    .AddInMemoryApiScopes(config.ApiScopes)
    .AddInMemoryApiResources(config.ApiResources)
    .AddInMemoryClients(config.Clients)
    .AddDeveloperSigningCredential();

builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddBackgroundJobs(builder.Configuration);
builder.Services.AddKafka(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(e =>
{
    _ = e.MapDefaultControllerRoute();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.PrepareDataPopulation();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
