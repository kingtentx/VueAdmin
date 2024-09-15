using VueAdmin.Data.ExtModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueAdmin.Data
{
    /// <summary>
    /// 配置
    /// </summary>  
    public class AppSettings : ExtFullModifyModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string AppKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_5000)]
        public string AppValue { get; set; }

    }
}
