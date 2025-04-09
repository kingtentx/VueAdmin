using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VueAdmin.Data.ExtModel;

namespace VueAdmin.Data
{
    [Table("department")]
    public class Department : ExtFullModifyModel, IsDeleteModel, ISortModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }       
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 级联层级
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string CascadeId { get; set; }
        /// <summary>
        /// 是否子集
        /// </summary>
        public bool Leaf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Principal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }       
    }
}
