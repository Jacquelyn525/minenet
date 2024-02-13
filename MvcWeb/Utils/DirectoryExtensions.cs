namespace System.IO {
  public static class DirectoryExtensions {
    public static IEnumerable<string> ArchiveDbPaths(string fsDirectory) {
      return Directory.EnumerateFiles(fsDirectory)
          .Where(p => p.Replace($"{fsDirectory}\\", "")
          .Length == 11 && p.ToLower().EndsWith(".db"));
    }
  }
}
