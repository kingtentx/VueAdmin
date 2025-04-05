using Org.BouncyCastle.Bcpg;

namespace VueAdmin.Api.Dtos
{
    public class UserRoleDto
    {
        public int Id{ get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int[] RoleIds { get; set; }
       
    }
}
