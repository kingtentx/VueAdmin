namespace VueAdmin.Web.Config
{
    public class RabbitMQConfig
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VirtualHost { get; set; } = "/";
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
    }
}
