using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VueAdmin.Data;

namespace VueAdmin.Api.Dtos
{
    public class MenuDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 菜单类型 0-菜单 1-iframe 2-外链 3-按钮
        /// </summary>            
        public int MenuType { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>      
        public string Title { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>     
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 组件
        /// </summary>      
        public string Component { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string Redirect { get; set; }
        /// <summary>
        /// 图标
        /// </summary>    
        public string Icon { get; set; }

        /// <summary>
        /// 右侧图标
        /// </summary>      
        public string ExtraIcon { get; set; }
        /// <summary>
        /// 进场动画
        /// </summary>      
        public string EnterTransition { get; set; }
        /// <summary>
        /// 离场动画
        /// </summary>    
        public string LeaveTransition { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>       
        public string Auths { get; set; }
        /// <summary>
        /// 菜单激活    
        /// </summary>      
        public string ActivePath { get; set; }
        /// <summary>
        /// 外链地址
        /// </summary>           
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
    }
}
