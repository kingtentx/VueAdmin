

using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace VueAdmin.Api.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class PagedDto
    {

        public int Page { get; set; } = 1;

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
        public int Count { get; set; } = 0;
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> Items { get; set; }
    }
}
