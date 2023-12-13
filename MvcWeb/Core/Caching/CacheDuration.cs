namespace MvcWeb.Core.Caching {
  // NOTE: https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1069
  // Expressed in seconds
  public enum CacheDuration {
    ResponseCacheGraphCMSContent = 10 * 60,
    ResponseCacheSystemData = 20 * 60,

    GraphCMSContent = 60 * 60,
    PasswordReset = GraphCMSContent,
    BookingFlow = GraphCMSContent,

    UserContext = 2 * 60 * 60,
    SystemData = UserContext,

    AtlasAuthToken = 30 * 60,

    Nonvolatile = 24 * 60 * 60,
    ConfirmAccountToken = Nonvolatile * 7,
  }

  [SingletonService]
  public class CacheDurationHelper {
    private readonly Settings _settings;

    public CacheDurationHelper(Settings settings) {
      _settings = settings;
    }

    public static int GetTimeInSeconds(Settings settings, CacheDuration duration)
        => settings.CacheDurations.TryGetValue(duration.ToString(), out var overrideValue) ? overrideValue : (int)duration;

    public static TimeSpan GetTimeSpan(Settings settings, CacheDuration duration)
        => TimeSpan.FromSeconds(GetTimeInSeconds(settings, duration));

    public int GetTimeInSeconds(CacheDuration duration)
        => GetTimeInSeconds(_settings, duration);

    public TimeSpan GetTimeSpan(CacheDuration duration)
        => GetTimeSpan(_settings, duration);
  }
}
