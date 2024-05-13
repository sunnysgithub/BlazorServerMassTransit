using System.Reflection;
using BlazorServerMassTransit.Components;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMassTransit(c =>
{
    c.SetKebabCaseEndpointNameFormatter();
    c.SetInMemorySagaRepositoryProvider();
    
    var entryAssembly = Assembly.GetEntryAssembly();
    c.AddConsumers(entryAssembly);
    c.AddSagaStateMachines(entryAssembly);
    c.AddSagas(entryAssembly);
    c.AddActivities(entryAssembly);
    
    c.UsingInMemory((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});

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