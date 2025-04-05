using System;

namespace VueAdmin.Api.Dtos
{
    public class AttachmentsDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>       
        public string FileName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>       
        public string Url { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>       
        public string ExtensionName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
    }
}
