using System.Runtime.Versioning;

namespace System.DirectoryServices {
  public static class ResultPropertyValueCollectionExtensions {

    [SupportedOSPlatform("windows")]
    public static string ToStringOrEmpty(this ResultPropertyValueCollection result) {
      if (result != null) {
        if (result.Count > 0) {
          var str = result![0].ToString();
          return str != null && str.Length > 0 ? str : string.Empty;
        }
      }
      return string.Empty;
    }
  }
}
