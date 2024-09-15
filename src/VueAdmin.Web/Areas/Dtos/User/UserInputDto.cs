using System.ComponentModel.DataAnnotations;

namespace VueAdmin.Web.Areas.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInputDto
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
        /// <summary>
        /// key
        /// </summary>        
        public string ValidateKey { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>       
        public string ValidateValue { get; set; }
    }
}
