using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace VueAdmin.Data
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public class DataInitializer
    {

        public void Create(AppDbContext context)
        {
            InitUser(context);
            //InitMenu(context);
        }

        private void InitUser(AppDbContext context)
        {
            string SuperAdmin = "admin";
            var system_user = context.User.FirstOrDefault(p => p.UserName.Equals(SuperAdmin));
            if (system_user == null)
            {
                //添加管理员  
                var admin = new User
                {
                    UserName = SuperAdmin,
                    RealName = "超级管理员",
                    Password = ToMD5("admin123"),                   
                    IsAdmin = true,
                    IsActive = true
                };
                context.User.Add(admin);
                context.SaveChanges();
            }
        }

        private void InitMenu(AppDbContext context)
        {
            string main_menu = "首页";
            var system_menu = context.Menu.FirstOrDefault(p => p.Title.Equals(main_menu));
            if (system_menu == null)
            {
                var main = new Menu { Pid = 0, Title = "首页", Icon = "layui-icon-home", Path = "/admin/main", MenuType = 2, Spread = true, IsShow = true, Sort = 0 };
                context.Menu.Add(main);
                context.SaveChanges();

                var system = new Menu { Pid = 0, Title = "系统管理", Icon = "layui-icon-set", PermissionKey = "System", MenuType = 1, Spread = false, IsShow = true,  Sort = 90 };
                context.Menu.Add(system);
                context.SaveChanges();

                var menus = new List<Menu>
                {
                    new Menu { Pid=system.Id, Title = "角色管理",Icon="layui-icon-user",Path="/role/index",PermissionKey="System_Role",Buttons="Add,Edit,Delete,Authorize", MenuType=2,Spread=false , IsShow=true,Sort=91},
                    new Menu { Pid=system.Id, Title = "管理员",Icon="layui-icon-username",Path="/manager/index",PermissionKey="System_Admin",Buttons="Add,Edit,Delete", MenuType=2,Spread=false , IsShow=true,Sort=92},
                    new Menu { Pid=system.Id, Title = "菜单管理",Icon="layui-icon-align-left",Path="/menu/index",PermissionKey="System_Menu",Buttons="Add,Edit,Delete", MenuType=2,Spread=false , IsShow=true,Sort=93},
                    new Menu { Pid=system.Id, Title = "登录日志",Icon="layui-icon-log",Path="/manager/logininfo",PermissionKey="System_LoginInfo", MenuType=2,Spread=false , IsShow=true,Sort=94}
                };
                context.Menu.AddRange(menus);
                context.SaveChanges();
            }
        }

        private string ToMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes_out = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            string result = BitConverter.ToString(bytes_out).Replace("-", "");
            return result;
        }
    }
}
