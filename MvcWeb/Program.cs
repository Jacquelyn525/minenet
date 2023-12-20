using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

using FluentValidation.AspNetCore;
using Newtonsoft.Json.Serialization;
using Serilog;

using MvcWeb.Models;
using MvcWeb.Hubs;
using MvcWeb.WS;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.SignalR;

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
  #region Services

  #region Entity Framework

  var connectionString = builder.Configuration.GetConnectionString("MineNetHistoryContext");

  builder.Services.AddDbContext<MineNetDBContext>(options =>
      options.UseSqlServer(connectionString));

  //builder.Services.AddDbContext<MinerEntryContext>(
  //     options => options.UseSqlServer("name=ConnectionStrings:MinerEntryContext"));

  builder.Services.AddDatabaseDeveloperPageExceptionFilter();

  #endregion Entity Framework

  builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        builder => {
          builder.WithOrigins("https://example.com")
              .AllowAnyHeader()
              .WithMethods("GET", "POST")
              .AllowCredentials();
        });
  });

  #region Fluent Validation
  builder.Services.AddFluentValidationAutoValidation()
    .AddControllers()
    .AddNewtonsoftJson(options => {
      options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });
  FluentValidation.ValidatorOptions.Global.DefaultClassLevelCascadeMode = FluentValidation.ValidatorOptions.Global.DefaultRuleLevelCascadeMode = FluentValidation.CascadeMode.Stop;
  #endregion

  #region Templated MVC Svcs

  builder.Services.AddControllersWithViews();

  builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
  builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = options.DefaultPolicy;
  });

  builder.Services.AddRazorPages();

  #endregion

  #region SignalR

  //builder.Services.AddHostedService<SchedulerService>();

#if DEBUG
  builder.Services.AddSignalR(o => o.EnableDetailedErrors = true);
#else
  builder.Services.AddSignalR();
#endif
  builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

  #endregion SignalR and Swagger

  #endregion Services


  #region Configure & Build App Services

  DiConfiguration.ConfigureServices(builder.Services, settings);
  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  #endregion

  #region Templated MVC

  app.UseHttpsRedirection();
  app.UseStaticFiles();

  app.UseRouting();

  app.UseAuthentication();
  app.UseAuthorization();

  #endregion

  #region SignalR

#pragma warning disable ASP0014 // Suggest using top level route registrations
  //app.UseEndpoints(endpoints => {
  //  endpoints.MapHub<MineNetHub>("/ws");
  //  endpoints.MapHub<ChatHub>("/chatHub");
  //});
#pragma warning restore ASP0014 // Suggest using top level route registrations


  #endregion SignalR

  #region Templated MVC
  app.MapHub<ChatHub>("/chatHub");
  app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");

  app.Run();


  #endregion

  #region Catch and Finally

} catch (Exception ex) {
  Log.Fatal(ex, "Unhandled Exception");
} finally {
  Log.Information("Shut down complete");
  Log.CloseAndFlush();
}

#endregion Catch and Finally
