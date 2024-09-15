namespace VueAdmin.Web
{
    /// <summary>
    /// 通用常量
    /// </summary>
    public class CacheKey
    {
        public const int ExpirationTimeLen_2 = 2;

        public const int ExpirationTimeLen_5 = 5;

        public const int ExpirationTimeLen_100 = 100;


        #region 缓存

        /// <summary>
        /// 验证码
        /// </summary>
        public const string ValidateCode = "ValidateCode:";

        /// <summary>
        /// 菜单缓存键
        /// </summary>
        public const string PermissionMenu = "PermissionMenu:";

        #endregion

        /// <summary>
        /// 微信配置
        /// </summary>
        public const string WeChatConfig = "WeChatConfig";
    }
}
