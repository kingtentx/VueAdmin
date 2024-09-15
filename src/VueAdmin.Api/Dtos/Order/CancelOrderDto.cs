namespace VueAdmin.Api.Dtos
{
    public class CancelOrderDto
    {
        /// <summary>
        /// 中台订单号
        /// </summary>
        public string OrderSn { get; set; }
        /// <summary>
        /// 订单取消成功/取消失败的通知地址
        /// </summary>
        public string NoticeConcelUrl { get; set; }
        /// <summary>
        /// 13位的时间戳
        /// </summary>
        public long Once { get; set; }
    }
}
