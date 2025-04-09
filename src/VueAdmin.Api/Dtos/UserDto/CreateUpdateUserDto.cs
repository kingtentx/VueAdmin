using System.ComponentModel.DataAnnotations;

namespace VueAdmin.Api.Dtos
{
    public class CreateUpdateUserDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
       public int? parentId { get; set; }
        /// <summary>
        /// ID
        /// </summary>      
        public int? Id { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>     
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机
        /// </summary>  
        public string Phone { get; set; }
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
        public string Nickname { get; set; }
        /// <summary>
        /// 备注
        /// </summary>  
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Status { get; set; } = false;
        /// <summary>
        /// 0-男 1-女
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DepartmentId { get; set; }

    }
}
