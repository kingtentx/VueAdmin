using VueAdmin.Api.Models.MapperConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VueAdmin.Api.Filters;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using VueAdmin.Api.SingnalR;
using Microsoft.AspNetCore.Authorization;
using VueAdmin.Api.Permissions;

namespace VueAdmin.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        private readonly string SqlConnection = "Default";
        private readonly string AllowSpecificMethods = "AllowSpecificMethods";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region 数据访问
            services.AddDbContext<AppDbContext>(options =>
                 options.UseMySql(_configuration.GetConnectionString(SqlConnection), MySqlServerVersion.LatestSupportedServerVersion));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            if (Convert.ToBoolean(_configuration["Redis:IsEnabled"]))
            {
                services.AddSingleton<ICacheService>(new RedisHelper(new RedisCacheOptions
                {
                    Configuration = _configuration["Redis:Configuration"],
                    InstanceName = _configuration["Redis:InstanceName"]
                }));
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<IMemoryCache>(factory =>
                {
                    var cache = new MemoryCache(new MemoryCacheOptions());
                    return cache;
                });
                services.AddSingleton<ICacheService, CacheHelper>();
            }
            #endregion   

            #region  AutoMapper

            services.AddAutoMapper(typeof(AutomapperConfig));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region jwt认证
            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtConfig>(_configuration.GetSection("JwtSettings"));
            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtConfig();
            _configuration.Bind("JwtSettings", jwtSettings);

            services.AddAuthentication(options =>
            {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //主要是jwt  token参数设置
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,
                    //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    ValidateLifetime = true,
                    ////允许的服务器时间偏移量
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region 注入权限         
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            #endregion

            #region 注册Swagger服务
            services.AddSwaggerGen(options =>
            {
                // 添加文档信息
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api"                  
                });

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

                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "VueAdmin.Api.xml");
                options.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）

            });
            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificMethods, builder =>
                {
                    builder.WithOrigins(
                        _configuration["App:CorsOrigins"]
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

            //services.AddSingleton<IHostedService, LogisticsService>();
            //services.AddSignalR();
            services.AddSignalR(hubOptions =>
            {
                //服务器端向客户端 ping的间隔
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
            });
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

            app.UseRouting();
            //CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行。            

            //app.UseHttpsRedirection();           

            app.UseAuthentication();//一定要在这个位置（app.UseAuthorization()上面）jwt

            app.UseAuthorization();
            //app.UseCors();
            app.UseCors(AllowSpecificMethods);
            //app.UseCors(builder =>
            //  builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
            //);

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

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "VueAdmin API");
            });

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
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
