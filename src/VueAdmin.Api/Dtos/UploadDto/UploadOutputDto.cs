namespace VueAdmin.Api.Dtos.UploadDto
{
    public class UploadOutputDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>       
        public string FileName { get; set; }

        /// <summary>
        /// 文件访问路径
        /// </summary>       
        public string Url { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>     
        public string ExtensionName { get; set; }
    }
}
