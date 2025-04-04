namespace VueAdmin.Core.Enums
{


    /// <summary>
    /// 返回结果状态码
    /// </summary>
    public enum ResultCode
    {     
        /// <summary>
        /// 成功200
        /// </summary>
        Success = 200,
        /// <summary>
        /// 失败400
        /// </summary>
        Fail = 400,
        /// <summary>
        /// 无权限401
        /// </summary>
        Nopermit = 401,
        /// <summary>
        /// 访问次数达到上限403
        /// </summary>
        Limited = 403,
        /// <summary>
        /// 无该记录405
        /// </summary>
        NULL = 405,
        /// <summary>
        /// 参数不全406
        /// </summary>
        ParmsError = 406,
        /// <summary>
        /// 服务器处理失败500
        /// </summary>
        ServerError = 500,
    }

    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 0,
        /// <summary>
        /// 商家
        /// </summary>
        Tenant = 1,
    }

    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image = 0,

        /// <summary>
        /// 文件
        /// </summary>
        File = 1
    }
}
