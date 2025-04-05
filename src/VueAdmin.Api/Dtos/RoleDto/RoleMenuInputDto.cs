namespace VueAdmin.Api.Dtos
{
    public class RoleMenuInputDto
    {
        public int RoleId { get; set; }

        public List<int> MenuIds { get; set; }
    }
}
