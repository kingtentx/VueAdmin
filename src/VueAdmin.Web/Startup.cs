using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using VueAdmin.Web.Config;
using VueAdmin.Web.Filters;
using VueAdmin.Web.Models.MapperConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VueAdmin.Web.Services;
using System.Linq;

namespace VueAdmin.Web
{
    public class Startup
    {

        private IConfiguration Configuration { get; }
        private readonly string SqlConnection = "Default";
        private readonly string AllowSpecificMethods = "AllowSpecificMethods";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 数据访问
            services.AddDbContext<AppDbContext>(options =>
                 options.UseMySql(Configuration.GetConnectionString(SqlConnection), MySqlServerVersion.LatestSupportedServerVersion),
                  contextLifetime: ServiceLifetime.Transient,
                  optionsLifetime: ServiceLifetime.Singleton);

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            ////注册DbContextFactory
            //services.AddDbContextFactory<AppDbContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString(SqlConnection), MySqlServerVersion.LatestSupportedServerVersion);
            //});
            #endregion

            #region  序列化数据
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不更改元数据的key的大小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss            
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            #endregion

            #region  缓存
            services.AddMemoryCache();

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            if (Convert.ToBoolean(Configuration["Redis:IsEnabled"]))
            {
                services.AddSingleton(typeof(ICacheService), new RedisHelper(new RedisCacheOptions
                {
                    Configuration = Configuration["Redis:Configuration"],
                    InstanceName = Configuration["Redis:InstanceName"]
                }));
            }
            else
            {
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheService, CacheHelper>();
            }
            #endregion  

            #region cookies jwt认证
            services.Configure<JwtConfig>(Configuration.GetSection("JwtSettings"));
            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置        
            var jwtSettings = new JwtConfig();
            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    //认证失败，会自动跳转到这个地址
                    options.LoginPath = "/Admin/ReLogin";
                    options.LogoutPath = "/Admin/ReLogin";
                })
                .AddJwtBearer(options =>
                {
                    //主要是jwt  token参数设置
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Token颁发机构
                        ValidIssuer = jwtSettings.Issuer,
                        //颁发给谁
                        ValidAudience = jwtSettings.Audience,
                        //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,
                        //允许的服务器时间偏移量
                        ClockSkew = TimeSpan.Zero,

                    };
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                //This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            #endregion

            #region 注册Swagger服务

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v2.0", Title = "King" });
                //获取应用程序所在目录(绝对,不受工作目录影响，建议采用此方法获取路径)
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer {Token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                ////项目属性生成配置的xml文件名
                //var xmlPath = Path.Combine(basePath, "VueAdmin.Web.xml");
                //options.IncludeXmlComments(xmlPath, true); //启用控制器注释
            });

            #endregion

            #region 注入组件
            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region 注入权限         
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificMethods, builder =>
                    {
                        builder.WithOrigins(
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        ) 
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
            #endregion

            #region 注入服务

            services.AddHostedService<JobService>();

            #endregion

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc(options =>
            {
                //options.Filters.Add<ActionFilter>();
                options.Filters.Add<ExceptionFilter>(); //加入全局异常类
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Admin/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(AllowSpecificMethods);
            app.UseAuthentication();//一定要在这个位置（app.UseAuthorization()上面）jwt
            app.UseAuthorization();
            //app.UseHttpsRedirection();

            #region 使用Swagger中间件
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    if (httpReq.Headers.ContainsKey("X-Request-Uri"))
                    {
                        var index = httpReq.Headers["X-Request-Uri"].ToString().IndexOf("/swagger/");
                        if (index > 0)
                        {
                            var serverUrl = $"{httpReq.Headers["X-Request-Uri"].ToString().Substring(0, index)}/";
                            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                        }
                    }
                });
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "VueAdminWeb API");
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");
            });

            #region 初始化数据
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();

            using (var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>())
            {
                //数据库是否存在:  true=未创建， false=已创建
                if (!dbContext.Database.EnsureCreated())
                {
                    new DataInitializer().Create(dbContext);//注册默认超级管理员
                }
            }
            #endregion
        }
    }
}
