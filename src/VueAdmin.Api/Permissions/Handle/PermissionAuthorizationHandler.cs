using Microsoft.AspNetCore.Authorization;
using StackExchange.Redis;
using System.Security.Claims;
using VueAdmin.Helper;

namespace VueAdmin.Api.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private IPermissionService _permission;

        public PermissionAuthorizationHandler(IPermissionService permission)
        {
            _permission = permission;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var identity = (ClaimsIdentity)context.User.Identity;
                if (Convert.ToBoolean(identity.FindFirst(ClaimTypes.System)?.Value))
                {
                    context.Succeed(requirement);
                }
                else
                {                   
                    var role = !string.IsNullOrWhiteSpace(identity.FindFirst(ClaimTypes.Role)?.Value) ? StringHelper.StrArrToIntArr(identity.FindFirst(ClaimTypes.Role)?.Value.ToString().Split(',')) : new int[] { 0 };
                    if (role != null)
                    {                                     
                        if (_permission.CheckPermission(role, requirement.Name))
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
