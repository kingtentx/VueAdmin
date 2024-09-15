using Microsoft.AspNetCore.Authorization;

namespace VueAdmin.Web
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(string name, PermissionType type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }

        public PermissionType Type { get; set; }
    }
}
