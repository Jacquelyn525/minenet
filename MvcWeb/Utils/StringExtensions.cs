namespace System {
  public static class StringExtensions {
    public static bool IsNullOrEmpty(this string value) {
      return string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrWhiteSpace(this string value) {
      return string.IsNullOrWhiteSpace(value);
    }

    public static string Left(this string s, int length) {
      if (s.IsNullOrEmpty()) {
        return string.Empty;
      }
      s = s.Trim();
      return s.Length < length ? s : s[..length];
    }

    public static string Right(this string s, int length) {
      if (s.IsNullOrEmpty()) {
        return string.Empty;
      }

      s = s.Trim();
      return s.Length < length ? s : s.Substring(s.Length - length, length);
    }

    public static string EmptyToValue(this string value, string emptyValue) {
      return value.IsNullOrWhiteSpace()
          ? emptyValue
          : value;
    }

    public static string TrimToEmpty(this string value) {
      return value != null
        ? value.Trim()
        : string.Empty;
    }

    public static bool EqualsIgnoreCase(this string value, string otherValue) {
      return value.TrimToEmpty().Equals(otherValue.TrimToEmpty(), StringComparison.OrdinalIgnoreCase);
    }
  }
}
