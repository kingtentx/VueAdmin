using Microsoft.EntityFrameworkCore;

namespace VueAdmin.Data
{
    public class AppDbContext : DbContext
    {
        //构造方法    
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region 数据区域

        public DbSet<User> User { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; } 
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<PictureGallery> PictureGallery { get; set; }
        public DbSet<Attachments> Attachments { get; set; }  
        public DbSet<User> WxUser { get; set; }

        #endregion

    }
}
