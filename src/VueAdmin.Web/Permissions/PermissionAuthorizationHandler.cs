using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VueAdmin.Web
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
                    //var role = context.User.FindFirst(_ => _.Type == ClaimTypes.Role).ToString();
                    var role = identity.FindFirst(ClaimTypes.Role)?.Value;
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        var code = requirement.Type == PermissionType.View ? requirement.Name : requirement.Name + "_" + requirement.Type;
                        if (_permission.CheckPermission(role, code))
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
