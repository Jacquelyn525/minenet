using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

using Serilog;

using MvcWeb.Hubs;
using SignalRChat.Hubs;

#region Logger

var cfg = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(cfg)
  .WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting up");

#endregion

#region Builder and Settings

var builder = WebApplication.CreateBuilder(args);
var settings = new Settings(builder.Configuration);

#endregion Builder and Settings

try {

  var connectionString = builder.Configuration.GetConnectionString("MineNetHistoryContext");

  builder.Services.AddDbContext<MvcWeb.Models.MineNetDBContext>(options => options.UseSqlServer(connectionString));
  builder.Services.AddDatabaseDeveloperPageExceptionFilter();

  builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
      builder => {
        builder.WithOrigins("https://example.com")
          .AllowAnyHeader()
          .WithMethods("GET", "POST")
          .AllowCredentials();
      });
  });

  builder.Services.AddControllersWithViews();
  builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
  builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });
  builder.Services.AddRazorPages();

  //builder.Services.AddHostedService<SchedulerService>();

#if DEBUG
  builder.Services.AddSignalR(o => o.EnableDetailedErrors = true);
#else
  builder.Services.AddSignalR();
#endif

  DiConfiguration.ConfigureServices(builder.Services, settings);
  var app = builder.Build();

  if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
  }

  app.UseHttpsRedirection();
  app.UseStaticFiles();
  app.UseRouting();
  app.UseAuthorization();
  app.UseCors();
  app.MapHub<MineNetHub>("/ws");
  app.MapHub<ChatHub>("/chatHub");
  app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

  app.Run();


  #region Catch and Finally

} catch (Exception ex) {
  Log.Fatal(ex, "Unhandled Exception");
} finally {
  Log.Information("Shut down complete");
  Log.CloseAndFlush();
}

#endregion Catch and Finally
