using Microsoft.AspNetCore.Authorization;
using StackExchange.Redis;

namespace VueAdmin.Api.Permissions
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(string name)
        {
            Name = name;             
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }          
    }
}
