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
                    NickName = "超级管理员",
                    Password = ToMD5("admin123"),
                    IsAdmin = true,
                    IsActive = true,
                    Roles = "1"
                };
                context.User.Add(admin);
                context.SaveChanges();

                var system_role = context.User.FirstOrDefault(p => p.Id == 1);
                if (system_role == null)
                {
                    //添加角色
                    var role = new Role
                    {
                        Code = SuperAdmin,
                        Name = "超级管理员",
                        IsActive = true
                    };
                    context.Role.Add(role);
                    context.SaveChanges();
                }
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
