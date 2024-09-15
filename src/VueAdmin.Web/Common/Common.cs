using Microsoft.AspNetCore.Http;
using VueAdmin.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using VueAdmin.Data;

namespace VueAdmin.Web
{
    public class Common
    {      
        /// <summary>
        /// 获取IP
        /// </summary>
        public static string GetIPAddress()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var ip = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();//X-Forwarded-For可能会包含多个IP
            if (string.IsNullOrEmpty(ip))
            {
                return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                return ip.IndexOf(',') > 0 ? ip.Split(',')[0] : ip;
            }
        }

        /// <summary>
        /// 菜单递归
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static List<TreeSelectModel> TreeSelect(List<Menu> list, TreeSelectModel tree)
        {
            int parentId = tree.Id;//根节点ID

           var treeList = new List<TreeSelectModel>();

            var children = list.Where(t => t.Pid == parentId);
            foreach (var chl in children)
            {
                var model = new TreeSelectModel();
                model.Id = chl.Id;
                model.Name = chl.Title;
                model.Sort = chl.Sort;

                var nodes = TreeSelect(list, model);
                model.Children = nodes.Count() > 0 ? nodes : null;
                treeList.Add(model);
            }
            return treeList;
        }
      

        /// <summary>
        /// 文件保存到服务器
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="serverPath"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public static async Task<bool> Save(Stream stream, string serverPath, string saveName)
        {
            try
            {
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                await Task.Run(() =>
                {
                    using (FileStream fs = new FileStream(serverPath + saveName, FileMode.Create))
                    {
                        stream.Position = 0;
                        stream.CopyTo(fs);
                        fs.Close();
                    }
                });
                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
