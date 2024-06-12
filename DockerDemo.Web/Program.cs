using DockerDemo.Components;
using DockerDemo.Web;
using DockerDemo.Web.Startup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<DemoDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("postgresdb"),
            npgsqlBuilder =>
            {
                npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            }));


builder.Services.AddSingleton<DemoDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<DemoDbInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<DemoDbInitializerHealthCheck>("DbInitializer", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
