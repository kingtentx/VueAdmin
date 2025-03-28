using System.ComponentModel.DataAnnotations;
using VueAdmin.Data;

namespace VueAdmin.Api.Dtos
{
    public class UserDto
    {
        /// <summary>
        /// 管理员ID
        /// </summary>      
        public int Id { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>     
        public string UserName { get; set; }
      
        /// <summary>
        /// 手机
        /// </summary>  
        public string Telphone { get; set; }
        /// <summary>
        /// Email
        /// </summary>  
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>     
        public string Avatar { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary> 
        public string RealName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>  
        public string Remark { get; set; }
        /// <summary>
        /// 角色,多角色英文逗号隔开
        /// </summary>
        public string Roles { get; set; }
     
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
