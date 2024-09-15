namespace VueAdmin.Api.Dtos
{
    public class ProductInputDto
    {
        /// <summary>
        /// 当前产品页数
        /// </summary>
        public int PageNo { get; set; } = 1;
        /// <summary>
        /// 13位的时间戳
        /// </summary>
        public long Once { get; set; }
    }
}
