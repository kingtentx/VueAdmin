﻿namespace VueAdmin.Api.Dtos
{
    public class GetUserInputDto : PagedDto
    {
        public int? DeptId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }      
        public bool? Status { get; set; }
    }
}
