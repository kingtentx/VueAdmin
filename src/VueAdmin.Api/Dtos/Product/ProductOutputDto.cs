namespace VueAdmin.Api.Dtos
{
    public class ProductOutputDto : PagedDto
    {
        public List<ProductDto> Result { get; set; }
    }
}
