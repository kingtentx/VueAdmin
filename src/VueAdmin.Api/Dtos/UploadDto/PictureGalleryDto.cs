namespace VueAdmin.Api.Dtos.UploadDto
{
    public class PictureGalleryDto

    {
        public long Id { get; set; }

        public string ImageName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>    
        public string Url { get; set; }
        /// <summary>
        /// 缩略图路径
        /// </summary>      
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>       
        public string ExtensionName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 原文件宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 原文件高度
        /// </summary>
        public int Height { get; set; }       
       
    }
}
