namespace System {

  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ParadoxDbModelAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Property)]
  public sealed class ParadoxDbColumnAttribute : Attribute {
    public string ColumnName { get; private set; } = string.Empty;
    public ParadoxDbColumnAttribute(string ColumnName) => this.ColumnName = ColumnName;

    public ParadoxDbColumnAttribute() { }
  }

}
