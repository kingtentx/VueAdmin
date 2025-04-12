using VueAdmin.Api.Dtos;

namespace VueAdmin.Api.Permissions
{
    public interface IPermissionService
    {
        /// <summary>
        /// 检查菜单权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="key"></param>      
        /// <returns></returns>
        bool CheckPermission(int[] role, string key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> GetPermissionKeys();     
    }
}
