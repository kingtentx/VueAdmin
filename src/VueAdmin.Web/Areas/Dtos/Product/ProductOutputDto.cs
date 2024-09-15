using System.Collections.Generic;

namespace VueAdmin.Web.Areas.Dtos.Product
{
    public class ProductOutputDto : PagedDto
    {
        public List<ProductDto> Result { get; set; }
    }
}
