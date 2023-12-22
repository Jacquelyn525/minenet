using System.Reflection;

namespace Microsoft.Extensions.Configuration {
  public static class ConfigurationExtensions {
    public static T GetSectionAs<T>(this IConfiguration config, string name)
      where T : class {
      var section = config.GetSection(name);
      if (section.Exists()) {
        return section.Get<T>();
      }

      return null;
    }

    public static string FromValuesOrRoot(this IConfiguration config, string name, string def = null) {
      return config[$"Values:{name}"] ?? config[name] ?? def;
    }

    public static void SetProps<T>(this IConfiguration configuration, T target, params string[] propsToSkip) {
      // only grab properties declared on the type
      typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Where(prop => !propsToSkip.Contains(prop.Name)).ToList()
        .ForEach(
          prop => {
            if (prop.SetMethod == null) {
              return; // readonly property
            }

            var propType = prop.PropertyType;

            if (propType == typeof(string)) {
              prop.SetMethod.Invoke(
                target,
                new object[] { configuration[prop.Name] }
              );
            } else if (propType == typeof(bool)) {
              prop.SetMethod.Invoke(
                target,
                new object[] { EmptyToFalse(configuration[prop.Name]) }
                );
            } else if (propType == typeof(string[])) {
              prop.SetMethod.Invoke(
                target,
                new object[] { Split(configuration[prop.Name]) }
              );
            } else if (propType == typeof(int)) {
              prop.SetMethod.Invoke(
                target,
                new object[] { configuration.GetValue(prop.Name, 0) }
              );
            } else if (propType == typeof(double)) {
              prop.SetMethod.Invoke(
                target, new object[] { configuration.GetValue(prop.Name, 0.0) }
              );
            } else if (propType == typeof(Dictionary<string, string>)) {
              prop.SetMethod.Invoke(
                target,
                new object[] { GetSectionAs<Dictionary<string, string>>(configuration, prop.Name) ?? new() }
              );
            } else if (propType == typeof(Dictionary<string, int>)) {
              prop.SetMethod.Invoke(
                target,
                new object[] { GetSectionAs<Dictionary<string, int>>(configuration, prop.Name) ?? new() }
              );
            }
          }
        );
    }

    private static string[] Split(string s) => string.IsNullOrEmpty(s) ? Array.Empty<string>() : s.Split(',', StringSplitOptions.RemoveEmptyEntries);


    /// <summary>
    /// configuration.GetValue uses the default if the key is missing, not if it is empty
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static bool EmptyToFalse(string s) => !string.IsNullOrEmpty(s) && Convert.ToBoolean(s);
  }
}
