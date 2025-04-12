using Serilog;
using StackExchange.Redis;
using System.Reflection;
using VueAdmin.Api.Dtos;
using VueAdmin.Core;
using VueAdmin.Core.Enums;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;

namespace VueAdmin.Api.Permissions
{
    public class PermissionService : IPermissionService
    {
        private IRepository<RoleMenu> _roleMenuRepository;
        private IRepository<Menu> _menuRepository;
        private ICacheService _cache;

        public PermissionService(
            IRepository<RoleMenu> roleMenuRepository,
            IRepository<Menu> menuRepository,
            ICacheService cache
            )
        {
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
            _cache = cache;
        }


        public bool CheckPermission(int[] role, string key)
        {
            try
            {
                var list = this.GetRolePermission(role).Result;
                return list.Contains(key, StringComparer.OrdinalIgnoreCase) ? true : false;
            }
            catch (Exception ex)
            {
                Log.Error("检查权限异常CheckPermission" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<string> GetPermissionKeys()
        {
            var permissions = new List<string>();
            Type permissionsCodeType = typeof(AuthorizeCode);

            foreach (Type nestedType in permissionsCodeType.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
            {
                foreach (FieldInfo field in nestedType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    // 新增过滤条件：排除名为 "Default" 的字段
                    if (field.Name != "Default" &&
                        field.FieldType == typeof(string) &&
                        field.IsLiteral)
                    {
                        string value = (string)field.GetValue(null);
                        permissions.Add(value);
                    }
                }
            }

            return permissions;
        }

        /// <summary>
        /// 角色的菜单权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>      
        private async Task<List<string>> GetRolePermission(int[] role)
        {
            var auths = new List<string>();
            var strKey = CacheKey.PermissionMenu + string.Join(",", role);

            if (_cache.Exists(strKey))
            {
                auths = _cache.Get<List<string>>(strKey);
            }
            else
            {
                var query = from role_menu in await _roleMenuRepository.GetListAsync(p => role.Contains(p.RoleId))
                            join menu in await _menuRepository.GetListAsync(p => p.MenuType == (int)MenuType.Button && p.IsDelete == false)
                            on role_menu.MenuId equals menu.Id
                            select menu;

                auths = query.Select(p => p.Auths).ToList();
                _cache.Add(strKey, auths, TimeSpan.FromHours(10), true);
            }

            return auths;

        }
    }
}
