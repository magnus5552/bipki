using System.Formats.Asn1;
using Bipki.App.Features.Activity;
using Bipki.App.Features.Auth;
using Bipki.App.Features.Chats;
using Bipki.App.Features.Chats.Hubs;
using Bipki.App.Features.Notifications;
using Bipki.App.Options;
using Bipki.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

builder.Services.AddAllOptions();

var appOptions = ApplicationOptionsProvider.GetConfiguration().Get<ApplicationOptions>();
builder.Services.AddDatabaseServices(appOptions!.DatabaseOptions.DbConnectionString);

builder.Services.AddIdentityServices();
builder.Services.AddChats();
builder.Services.AddActivitiesServices();
builder.Services.AddWebPushServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UsePathBase("/api");


app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chats-hub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

using var scope = app.Services.CreateScope();
var waitService = scope.ServiceProvider.GetRequiredService<ZalupaService>();
using(var cts = new CancellationTokenSource())
{
    app.Lifetime.ApplicationStopping.Register(() => cts.Cancel());
    _ = waitService.Start(cts.Token);
}
app.Run();