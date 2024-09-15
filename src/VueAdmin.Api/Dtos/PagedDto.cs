

namespace VueAdmin.Api.Dtos
{
    public class PagedDto
    {

        /// <summary>
        /// 数据总条数
        /// </summary>
        public long TotalCount { get; set; } = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public long PageNo
        {
            get
            {
                return TotalCount % pageSize > 0 ? TotalCount / pageSize + 1 : TotalCount / pageSize;
            }
            set
            {
                this.pageIndex = value;
            }
        }
        private long pageIndex = 1;

        private long pageSize = 20;
    }
}
