using VueAdmin.Data.ExtModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueAdmin.Data
{
    /// <summary>
    /// 角色
    /// </summary> 
    [Table("role")]
    public class Role : ExtFullModifyModel, IActiveModel, IsDeleteModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 角色类型 admin [超级管理员]
        /// </summary>
        public string Code { get; set; }
     

        public bool IsDelete { get; set; }
    }
}