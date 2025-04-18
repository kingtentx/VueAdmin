﻿using VueAdmin.Data.ExtModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VueAdmin.Data
{
    [Table("menu")]
    public class Menu : ExtFullModifyModel, IsDeleteModel, ISortModel
    {        
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 菜单类型 0-菜单 1-iframe 2-外链 3-按钮
        /// </summary>      
        [Comment("菜单类型 0-菜单 1-iframe 2-外链 3-按钮")]
        public int MenuType { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Title { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [StringLength(ModelUnits.Len_500)]
        public string Path { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Component { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(ModelUnits.Len_250)]
        public string Redirect { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string Icon { get; set; }

        /// <summary>
        /// 右侧图标
        /// </summary>
        [StringLength(ModelUnits.Len_50)]
        public string ExtraIcon { get; set; }
        /// <summary>
        /// 进场动画
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string EnterTransition { get; set; }
        /// <summary>
        /// 离场动画
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string LeaveTransition { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string Auths { get; set; }
        /// <summary>
        /// 菜单激活    
        /// </summary>
        [StringLength(ModelUnits.Len_100)]
        public string ActivePath { get; set; }
        /// <summary>
        /// 外链地址
        /// </summary>      
        [StringLength(ModelUnits.Len_100)]
        public string FrameSrc { get; set; }
        /// <summary>
        ///  加载动画
        /// </summary>        
        public bool FrameLoading { get; set; }        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 缓存页面
        /// </summary>    
        public bool KeepAlive { get; set; }
        /// <summary>
        /// 标签页
        /// </summary>
        public bool HiddenTag { get; set; }
        /// <summary>
        /// 固定标签页
        /// </summary>
        public bool FixedTag { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool ShowLink { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowParent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; } = false;
     
    }
}
