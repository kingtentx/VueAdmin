

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
        public string NickName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>  
        public string Remark { get; set; }

        ///// <summary>
        ///// 角色,多角色英文逗号隔开
        ///// </summary>
        //public string Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationTime { get; set; }

        public DeptDto Dept { get; set; } 
    }

    public class DeptDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
