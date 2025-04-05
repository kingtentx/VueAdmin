using System.ComponentModel.DataAnnotations;

namespace VueAdmin.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 手机
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        ///// <summary>
        ///// key
        ///// </summary>        
        //public string ValidateKey { get; set; }
        ///// <summary>
        ///// 验证码
        ///// </summary>       
        //public string ValidateValue { get; set; }
    }

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
        /// <summary>
        /// 
        /// </summary>
        public int[] Role { get; set; }

    }
}
