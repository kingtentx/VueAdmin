namespace VueAdmin.Api.Dtos.User
{
    public class CreateUpdateUserDto
    {
        /// <summary>
        /// ID
        /// </summary>      
        public int? Id { get; set; }

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
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
       
    }
}
