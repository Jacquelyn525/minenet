using System.Text.Json;

namespace MvcWeb {
  public class Constants {
    protected Constants() { }

    public static readonly JsonSerializerOptions DefaultSerializerOptions = new(JsonSerializerDefaults.Web);

    public static class Environments {
      public const string Prod = "prod";
      public const string Local = "local";
    }

    public static class Auth {
      public static class Claims {
        public const string Email = "email";
        public const string DisplayName = "displayname";
        public const string UserName = "username";
        public const string FirstName = "firstname";
        public const string LastName = "lastname";
        public const string Role = "role";
      }

      public static class RoleCategories {
        public const string Admin = nameof(Admin);
      }

      public static class Roles {
        public const int Admin = 1;
      }

      public static class Permissions {
        public const string Admin = nameof(Admin);
      }
    }

    public static class ResponseCacheProfiles {
      public const string SystemData = nameof(SystemData);
      public const string Nonvolatile = nameof(Nonvolatile);
      public const string Never = nameof(Never);
    }

  }
}
