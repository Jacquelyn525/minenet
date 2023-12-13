namespace MvcWeb.Core {
  public class ErrorCodeException : Exception {
    private const string _argsKey = "ErrorCodeArgs";
    public ErrorCodeException(string errorCode, Exception innerException = null)
      : base(errorCode, innerException) { }

    public ErrorCodeException(string errorCode, params string[] args)
      : this(errorCode) {
      Args = args;
    }

    public string[] Args {
      get => Data[_argsKey] as string[];
      set => Data[_argsKey] = value;
    }
  }
}
