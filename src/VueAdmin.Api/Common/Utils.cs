using VueAdmin.Api.Permissions;
using VueAdmin.Data;

namespace VueAdmin.Api
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

        /// <summary>
        /// 将 Base64 字符串转换为MemoryStream 
        /// </summary>
        /// <param name="strBase64"></param>
        /// <returns></returns>
        public static Stream ConvertBase64ToStream(string strBase64)
        {
            //1.把头部的data:image/png;base64,（注意有逗号）去掉
            string base64 = strBase64.Substring(strBase64.IndexOf(',') + 1);
            // 2.将 Base64 字符串转换为字节数组  
            byte[] bytes = Convert.FromBase64String(base64);
            // 3.将字节数组转换为 MemoryStream  
            MemoryStream memoryStream = new MemoryStream(bytes);
            return memoryStream;
        }

        /// <summary>
        /// 通过图片base64流判断图片等于多少字节 image图片流
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static long ImageSize(string image)
        {
            int i = image.IndexOf(',') + 1; //1.把头部的data:image/png;base64,（注意有逗号）去掉。
            string str = image.Substring(i);

            int equalIndex = str.IndexOf("=");//2.找到等号，把等号也去掉
            if (str.IndexOf("=") > 0)
            {
                str = str.Substring(0, equalIndex);
            }
            int strLength = str.Length;//3.原来的字符流大小，单位为字节
            int size = strLength - (strLength / 8) * 2;//4.计算后得到的文件流大小，单位为字节
            return (long)size;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPermissionsCode(string menu, PermissionType type)
        {
           return $"{menu}:btn:{type.ToString()}";
        }
    }
}
