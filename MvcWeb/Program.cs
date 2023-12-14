using System.Runtime.Versioning;
using System.Text;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

using FluentValidation.AspNetCore;
using Newtonsoft.Json.Serialization;
using Serilog;

using MvcWeb.WS;



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

  //var connectionString = builder.Configuration.GetConnectionString("MinerEntryContext");

  //builder.Services.AddDbContext<MvcWeb.Data.MinerEntryContext>(options =>
  //    options.UseSqlServer(connectionString));

  // builder.Services.AddDbContext<MinerEntryContext>(
  //      options => options.UseSqlServer("name=ConnectionStrings:MinerEntryContext"));

  builder.Services.AddDatabaseDeveloperPageExceptionFilter();

  #endregion Entity Framework

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
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
  });
  builder.Services.AddRazorPages();

  #endregion

  #region SignalR and Swagger

  //builder.Services.AddHostedService<SchedulerService>();

#if DEBUG
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  builder.Services.AddSignalR(o => o.EnableDetailedErrors = true);
#else
        builder.Services.AddSignalR();
#endif
  //o.KeepAliveInterval
  //o.ClientTimeoutInterval
  //o.SupportedProtocols
  //o.MaximumParallelInvocationsPerClient

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

  app.UseAuthorization();

  #endregion

  #region SignalR

#pragma warning disable ASP0014 // Suggest using top level route registrations
  app.UseEndpoints(endpoints => {
    endpoints.MapHub<MineNetHub>("/ws");
  });
#pragma warning restore ASP0014 // Suggest using top level route registrations


  #endregion SignalR

  #region Templated MVC

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
