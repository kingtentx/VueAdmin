namespace VueAdmin.Api.Dtos
{
    public class UserInfoDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 小铭
        /// </summary>
        public string NickName { get; set; }
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
