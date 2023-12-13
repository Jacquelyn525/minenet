using System.Runtime.Versioning;
using System.Text;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Net.Http.Headers;

using FluentValidation.AspNetCore;

using Newtonsoft.Json.Serialization;

using Serilog;


#region Logger

var cfg = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(cfg)
  .WriteTo.Console().CreateBootstrapLogger();
Log.Information("Starting up");

#endregion

var builder = WebApplication.CreateBuilder(args);
var settings = new Settings(builder.Configuration);


try {

  // Add services to the container.

  #region Fluent Validation
  builder.Services.AddFluentValidationAutoValidation()
    .AddControllers(options => options.AddResponseCacheProfiles(settings))
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

  DiConfiguration.ConfigureServices(builder.Services, settings);

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }
  

  #region Templated MVC

  app.UseHttpsRedirection();
  app.UseStaticFiles();

  app.UseRouting();

  app.UseAuthorization();

  app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");

  app.Run();

  #endregion

} catch (Exception ex) {
  Log.Fatal(ex, "Unhandled Exception");
} finally {
  Log.Information("Shut down complete");
  Log.CloseAndFlush();
}
