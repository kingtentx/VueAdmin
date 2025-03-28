namespace VueAdmin.Api.Dtos
{
    public class GetUserInputDto : PagedDto
    {
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
    }
}
