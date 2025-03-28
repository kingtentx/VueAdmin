﻿using VueAdmin.Data.ExtModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueAdmin.Data
{
    /// <summary>
    /// 角色菜单
    /// </summary>  
    [Table("role_menu")]
    public class RoleMenu : ExtCreateModel
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        [StringLength(ModelUnits.Len_100)]
        public string Permission { get; set; }

    }
}