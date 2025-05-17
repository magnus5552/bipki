using Bipki.App.Features.Auth;
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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();