namespace VueAdmin.Api.Dtos.UploadDto
{
    public class UploadFileDto
    {
        /// <summary>
        /// 文件类型 0-image 1-file
        /// </summary>
        public int FileType { get; set; }
        /// <summary>
        /// 上传路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 最大值 MB
        /// </summary>
        public long MaxSize { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string[] ExtendedName { get; set; }
    }
}
