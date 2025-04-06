namespace VueAdmin.Api.Dtos
{
    public class CreateUpdateDepartmentDto
    {
        /// <summary>
        /// 
        /// </summary>       
        public int? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>      
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Principal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>      
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>      
        public string Remark { get; set; }
    }
}
