using VueAdmin.Data.ExtModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueAdmin.Data
{
    /// <summary>
    /// ����Ա
    /// </summary>  
    [Table("user")]
    public class User : ExtFullModifyModel, IActiveModel, IsDeleteModel
    {
        /// <summary>
        /// ����ԱID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// ����Ա����
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string UserName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [StringLength(ModelUnits.Len_50)]
        public string Password { get; set; }
        /// <summary>
        /// �ֻ�
        /// </summary>      
        [StringLength(ModelUnits.Len_50)]
        public string Telphone { get; set; }
        /// <summary>
        /// Email
        /// </summary>      
        [StringLength(ModelUnits.Len_250)]
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Avatar { get; set; }
        /// <summary>
        /// ��ʵ����
        /// </summary>       
        [StringLength(ModelUnits.Len_50)]
        public string NickName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>       
        [StringLength(ModelUnits.Len_500)]
        public string Remark { get; set; }
        /// <summary>
        /// ��ɫ,���ɫӢ�Ķ��Ÿ���
        /// </summary>       
        [StringLength(ModelUnits.Len_500)]
        public string Roles { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>   
        public int DepartmentId { get; set; }
        /// <summary>
        /// �Ƿ񳬼�����Ա
        /// </summary>
        public bool IsAdmin { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public int? Sex { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
