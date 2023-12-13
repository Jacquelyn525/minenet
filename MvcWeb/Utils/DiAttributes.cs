namespace System {
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class TransientServiceAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Class)]
  public sealed class SingletonServiceAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ScopedServiceAttribute : Attribute { }
}
