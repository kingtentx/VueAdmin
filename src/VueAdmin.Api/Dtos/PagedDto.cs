

namespace VueAdmin.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class PagedDto
    {
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public int CurrentPage { get; set; } = 1;


    }
}
