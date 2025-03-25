using WebsiteOpcUa.Data;
var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Œcie¿ka do katalogu roboczego
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

IConfigurationSection getConfig = config.GetSection("AppSettings").GetSection("MySetting");


Influx db = new Influx(getConfig.Value, "dataopcua", "opcua", "http://localhost:8086");

// Add services to the container.
builder.Services.AddSingleton(db);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
