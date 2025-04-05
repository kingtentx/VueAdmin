using System.ComponentModel.DataAnnotations;
using VueAdmin.Data;

namespace VueAdmin.Api.Dtos
{
    public class RoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>     
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>       
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>    
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 角色类型 admin [超级管理员]
        /// </summary>
        public string Code { get; set; }
    }
}
