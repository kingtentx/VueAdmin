using System.ComponentModel;

namespace VueAdmin.Api.Permissions
{
    public enum PermissionType
    {
        /// <summary>
        /// 查看
        /// </summary>
        [Description("查看")]
        view = 0,
        /// <summary>
        /// 增加
        /// </summary>
        [Description("新增")]
        add = 1,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        edit = 2,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        delete = 3,
        /// <summary>
        /// 导入
        /// </summary>
        [Description("导入")]
        import = 4,
        /// <summary>
        /// 导出
        /// </summary>
        [Description("导出")]
        export = 5,
        /// <summary>
        /// 授权
        /// </summary>
        [Description("授权")]
        authorize = 6,
    }
}
