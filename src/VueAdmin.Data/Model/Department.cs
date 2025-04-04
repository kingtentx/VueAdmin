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
        public int? ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Remark { get; set; }
    }
}
