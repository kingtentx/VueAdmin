namespace VueAdmin.Api.Dtos
{
    public class LoginUserDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; } = 0;
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;

    }

    public class UserInfoDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 小铭
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Permissions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Expires { get; set; }
    }

}
