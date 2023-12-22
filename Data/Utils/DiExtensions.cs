using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

//using MineNet.Api.Services.ScheduledTasks;

using MvcWeb;

namespace Microsoft.Extensions.DependencyInjection {

  
  public static class DiExtensions {

    public static void RegisterServices(this IServiceCollection services, Assembly assembly, Settings settings) {
      foreach (var type in assembly.GetExportedTypes()) {
        if (type.GetCustomAttribute<TransientServiceAttribute>() != null) {
          services.AddTransient(type);
        } else if (type.GetCustomAttribute<SingletonServiceAttribute>() != null) {
          services.AddSingleton(type);
        } else if (type.GetCustomAttribute<ScopedServiceAttribute>() != null) {
          services.AddScoped(type);
        }
      }
    }
  }
}
