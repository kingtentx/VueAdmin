﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VueAdmin.Data.ExtModel;

namespace VueAdmin.Data
{
    /// <summary>
    /// 附件
    /// </summary>
    [Table("attachments")]
    public class Attachments : ExtCreateModel, ICreateByModel
    {
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string FileName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Url { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        [StringLength(ModelUnits.Len_10)]
        public string ExtensionName { get; set; }
        /// <summary>
        /// MD5值
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string MD5 { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string CreateBy { get; set; }
    }
}
