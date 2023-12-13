using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

//using FluentValidation;

namespace MvcWeb {
  public static class DiConfiguration {
    public static void ConfigureServices(IServiceCollection services, Settings settings) {
      if (services.Any(sd => sd.ServiceType == typeof(Settings))) {
        return; // keeps from loading multiple times
      }

      services.AddHttpClient(); // ??? not sure why either...
      services.AddSingleton(settings);

      var assembly = typeof(DiConfiguration).GetTypeInfo().Assembly;

      //services.AddValidatorsFromAssembly(assembly);
      services.RegisterServices(assembly, settings);
    }
  }
}
