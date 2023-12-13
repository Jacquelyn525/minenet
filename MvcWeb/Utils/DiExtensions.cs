using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

//using MineNet.Api.Services.ScheduledTasks;

using MvcWeb.Core.Data;

namespace Microsoft.Extensions.DependencyInjection {

  [SupportedOSPlatform("windows")]
  public static class DiExtensions {

    public static void RegisterServices(this IServiceCollection services, Assembly assembly, Settings settings) {
      var sqlServerFactory = typeof(DbAdapterFactory).GetMethod(nameof(DbAdapterFactory.GetConnectionAs));

      foreach (var type in assembly.GetExportedTypes()) {
        if (type.GetCustomAttribute<TransientServiceAttribute>() != null) {
          services.AddTransient(type);
        } else if (type.GetCustomAttribute<SingletonServiceAttribute>() != null) {
          services.AddSingleton(type);
        } else if (type.GetCustomAttribute<ScopedServiceAttribute>() != null) {
          services.AddScoped(type);
        } else if (type.GetCustomAttribute<DatabaseServiceAttribute>(true) is DatabaseServiceAttribute databaseServiceAttribute) {
          services.AddTransient(type, (serviceProvider) => {

            var connectionString = settings.ConnectionStrings[databaseServiceAttribute.ConnectionStringName];

            var genericMethod = sqlServerFactory!.MakeGenericMethod(type);
            return genericMethod.Invoke(null, new object[] { connectionString })!;
          });
        }
      }
    }


#if DEBUG
    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
#endif

  }
}
