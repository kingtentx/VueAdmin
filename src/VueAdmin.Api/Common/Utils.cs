namespace VueAdmin.Api.Common
{
    public class Utils
    {
        public static object _lock = new object();
        public static int count = 1;
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public static string CreateOrderNo()
        {
            lock (_lock)
            {
                if (count >= 10000)
                {
                    count = 1;
                }
                var number = "P" + DateTime.Now.Ticks + count.ToString("0000");
                count++;
                return number;
            }
        }


        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var ip = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();//X-Forwarded-For可能会包含多个IP
            if (string.IsNullOrEmpty(ip))
            {
                return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                return ip.IndexOf(',') > 0 ? ip.Split(',')[0] : ip;
            }
        }
    }
}
