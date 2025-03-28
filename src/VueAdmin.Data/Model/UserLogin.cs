using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueAdmin.Data
{
    /// <summary>
    /// 登录日志
    /// </summary> 
    [Table("user_login")]
    public class UserLogin
    {
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string UserName { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Client { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LoginDate { get; set; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string LoginIp { get; set; }
    }
}
