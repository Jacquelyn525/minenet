using Microsoft.AspNetCore.Authorization;

namespace MvcWeb.Core {
  public class AuthorizeRoleAttribute : AuthorizeAttribute {
    public AuthorizeRoleAttribute(params string[] roles) {
      Roles = string.Join(",", roles);
    }
  }
}
