namespace VueAdmin.Api.Dtos
{
    public class OrderInputDto
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductSn { get; set; }
        /// <summary>
        /// 中台用户唯一标识
        /// </summary>
        public string UserUuid { get; set; }
        /// <summary>
        /// 请求订单号（后续通知出券，邮寄状态等使用）
        /// </summary>
        public string OrderSn { get; set; }
        /// <summary>
        /// 异步出券通知地址
        /// </summary>
        public string DelayLoadUrl { get; set; }
        /// <summary>
        /// 优惠券使用通知地址
        /// </summary>
        public string UsedUrl { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户openid（只有微信立减金产品才会有这个字段）
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 收货人姓名（只有邮寄产品才会有这个字段）
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// 收货人联系电话（只有邮寄产品才会有这个字段）
        /// </summary>
        public string ReceiverTel { get; set; }
        /// <summary>
        /// 收货人联系地址（只有邮寄产品才会有这个字段）
        /// </summary>
        public string ReceiverAddress { get; set; }
        /// <summary>
        /// 物流回调地址（只有邮寄产品才会有这个字段）
        /// </summary>
        public string MailUrl { get; set; }
        /// <summary>
        /// 13位的时间戳
        /// </summary>
        public long Once { get; set; }
    }
}
