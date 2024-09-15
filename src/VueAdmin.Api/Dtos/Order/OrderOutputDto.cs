namespace VueAdmin.Api.Dtos
{
    public class OrderOutputDto
    {
        /// <summary>
        /// 原样返回的中台订单号
        /// </summary>
        public string orderSn { get; set; }
        /// <summary>
        /// 第三方订单号（后续中台订单与第三方订单一一对应使用）不超过50字符
        /// </summary>
        public string thirdOrderSn { get; set; }
        /// <summary>
        /// 是否延时出券，true：延时出券，false:试试出券
        /// 当延时出券时，下方参数不需要返回
        /// 当非延时出券时，接口类型至少满足一项
        /// 只有卡券类需要延时出券，非卡券类商品通过时候邮寄成功来回调
        /// </summary>
        public string delayLoad { get; set; }
        /// <summary>
        /// 接口类型：1:卡号，2：卡号卡密，3：链接，5：直冲，没有具体卡号/卡密/链接，6：实物礼品
        /// </summary>
        public string awardType { get; set; }
        /// <summary>       
        /// 卡号
        /// </summary>
        public string cardno { get; set; }
        /// <summary>
        /// 卡密
        /// </summary>
        public string cardpwd { get; set; }
        /// <summary>
        /// 链接/二维码地址（http开头，不支持base64）
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// yyyyMMdd 格式的有效期开始时间
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// yyyyMMdd 格式的有效期截止时间
        /// </summary>
        public string endDate { get; set; }


    }
}
