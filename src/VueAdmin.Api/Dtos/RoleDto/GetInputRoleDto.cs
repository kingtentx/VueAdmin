namespace VueAdmin.Api.Dtos
{
    public class GetInputRoleDto : PagedDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool? Status { get; set; }
    }
}
