using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Org.BouncyCastle.Utilities;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Helper.SM4;
using VueAdmin.Repository;

namespace VueAdmin.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Authorize]

    public class UserController : ApiBaseController
    {
        private ICacheService _cache;
        private IMapper _mapper;
        private IRepository<User> _userRepository;
        private IRepository<UserLogin> _logRepository;
        private IRepository<Role> _roleRepository;
        private IRepository<Department> _deptRepository;
        private JwtConfig _jwtSettings;

        public UserController(
            IMapper mapper,
            ICacheService cache,
            IOptions<JwtConfig> jwt,
            IRepository<User> userRepository,
            IRepository<UserLogin> logRepository,
            IRepository<Role> roleRepository,
            IRepository<Department> deptRepository
            )
        {
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtSettings = jwt.Value;
            _logRepository = logRepository;
            _roleRepository = roleRepository;
            _deptRepository = deptRepository;
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ResultDto<UserInfoDto>> Login(LoginDto input)
        {
            var result = new ResultDto<UserInfoDto>();

            if (!string.IsNullOrEmpty(input.UserName) && !string.IsNullOrEmpty(input.Password))//判断账号密码是否正确
            {
                var user = await _userRepository.GetOneAsync(p => p.UserName == input.UserName && p.Password == StringHelper.ToMD5(input.Password) && p.IsDelete == false);
                if (user == null)
                {
                    result.Msg = "用户启或密码错误";
                    return result;
                }
                if (!user.IsActive)
                {
                    result.Msg = "当前用户已禁用";
                    return result;
                }
                if (user.Roles.IsNullOrEmpty())
                {
                    user.Roles = "0";
                }
                var claim = new Claim[]{
                    new Claim(ClaimTypes.Sid,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role, user.Roles),
                    new Claim(ClaimTypes.System , user.IsAdmin.ToString())
                };

                //对称秘钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                //签名证书(秘钥，加密算法)
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claim, DateTime.Now, DateTime.Now.AddMinutes(_jwtSettings.Expiration), creds);

                //查询用户角色
                //var roles = new List<string>(); //new List<string> { "admin" };
                //if (user.Roles != "0")
                //{
                //    var roleIds = StringHelper.StrArrToIntArr(user.Roles.Split(','));
                //    roles = await _roleRepository.GetQueryable(p => roleIds.Contains(p.Id) && p.IsDelete == false && p.IsActive == true).Select(p => p.Code).ToListAsync();
                //}

                var info = new UserInfoDto
                {
                    UserName = user.UserName,
                    NickName = user.NickName,
                    Avatar = user.Avatar,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = "",
                    //Roles = roles,
                    Permissions = new List<string>(), // user.IsAdmin ? new List<string> { "*:*:*" } : new List<string>(),
                    Expires = DateTime.Now.AddMinutes(_jwtSettings.Expiration)
                };
                try
                {
                    var model = new UserLogin()
                    {
                        UserName = user.UserName,
                        Client = Request.Headers["User-Agent"].ToString(),
                        LoginDate = DateTime.Now,
                        LoginIp = Utils.GetIPAddress()
                    };
                    await _logRepository.AddAsync(model);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
                result.SetData(info);
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<ResultDto<string>> Logout()
        {
            var result = new ResultDto<string>();

            result.SetData("success");

            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("info")]
        public async Task<ResultDto<UserDto>> GetUserInfo(int id)
        {
            var result = new ResultDto<UserDto>();

            var query = await _userRepository.GetOneAsync(p => p.Id == id);
            var data = _mapper.Map<UserDto>(query);
            result.SetData(data);

            return result;
        }

        [HttpPost]
        [Route("list")]
        public async Task<ResultDto<ItemList<UserDto>>> GetUserList([FromBody] GetUserInputDto input)
        {
            #region Test
            //var result = new ResultDto<ItemList<object>>();
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "AppData/user.json");
            //if (!System.IO.File.Exists(filePath))
            //{
            //    result.Msg = "JSON 文件未找到。";
            //    return result;
            //}
            //string jsonContent = await System.IO.File.ReadAllTextAsync(filePath);
            //var itmes = JsonConvert.DeserializeObject<List<object>>(jsonContent);

            //var data = new ItemList<object>
            //{
            //    Total = 2,
            //    List = itmes
            //};
            //result.SetData(data);
            //return result;
            #endregion

            var result = new ResultDto<ItemList<UserDto>>();

            var where = LambdaHelper.True<User>().And(p => p.IsDelete == false);
            if (!string.IsNullOrEmpty(input.UserName))
            {
                where = where.And(p => p.UserName.Contains(input.UserName));
            }
            if (!string.IsNullOrEmpty(input.Phone))
            {
                where = where.And(p => p.Telphone.Equals(input.Phone));
            }
            if (input.Status.HasValue)
            {
                where = where.And(p => p.IsActive == input.Status);
            }
            if (input.DeptId.HasValue)
            {
                var dept = await _deptRepository.GetOneAsync(p => p.Id == input.DeptId && p.IsDelete == false);
                var deptIds = await _deptRepository.GetQueryable(p => p.CascadeId.StartsWith(dept.CascadeId) && p.IsDelete == false).Select(p => p.Id).ToListAsync();
                where = where.And(p => deptIds.Contains(p.DepartmentId));
            }

            var items = await _userRepository.GetListAsync(where, p => p.Id, input.CurrentPage, input.PageSize);
            var depts = await _deptRepository.GetListAsync(p => items.List.Distinct().Select(p => p.DepartmentId).Contains(p.Id) && p.IsDelete == false);

            var dtos = new List<UserDto>();
            foreach (var item in items.List)
            {
                var dto = _mapper.Map<UserDto>(item);
                dto.Dept = new DeptDto()
                {
                    Id = item.DepartmentId,
                    Name = depts.Find(p => p.Id == item.DepartmentId)?.Name
                };
                dtos.Add(dto);
            }
            var data = new ItemList<UserDto>
            {
                Total = items.Count,
                List = dtos,
                CurrentPage = input.CurrentPage,
                PageSize = input.PageSize
            };

            result.SetData(data);
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ResultDto<bool>> Create([FromBody] CreateUpdateUserDto input)
        {
            var result = new ResultDto<bool>();
            var query = await _userRepository.GetOneAsync(p => p.UserName.Equals(input.UserName) && p.IsDelete == false);
            if (query != null)
            {
                result.Msg = "用户名已存在";
                return result;
            }

            var model = _mapper.Map<User>(input);
            model.Password = StringHelper.ToMD5("00000000");
            model.CreateBy = LoginUser.UserName;

            var entity = await _userRepository.AddAsync(model);
            result.SetData(entity.Id > 0);
            return result;
        }

        [HttpPost]
        [Route("edit")]
        public async Task<ResultDto<bool>> Update([FromBody] CreateUpdateUserDto input)
        {
            var result = new ResultDto<bool>();
            var query = await _userRepository.GetOneAsync(p => p.UserName.Equals(input.UserName) && p.Id != input.Id && p.IsDelete == false);
            if (query != null)
            {
                result.Msg = "用户名已存在";
                return result;
            }

            var entity = _userRepository.GetQueryable(p => p.Id == input.Id && p.IsDelete == false).AsNoTracking().FirstOrDefault();
            if (entity == null)
            {
                result.Msg = "用户不存在";
                return result;
            }
            else
            {
                if (entity.UserName.Trim() != input.UserName.Trim())
                {
                    result.Msg = "用户名不能修改";
                    return result;
                }
            }

            #region 判断已登录用户禁止修改当前登录账号为禁用状态

            if (LoginUser.UserId == input.Id && !input.Status)
            {
                result.Msg = "用户禁止修改当前登录账号为禁用状态";
                return result;
            }

            #endregion

            var model = _mapper.Map<User>(input);
            if (string.IsNullOrWhiteSpace(input.Password))
            {
                model.Password = entity.Password;
            }
            else
            {
                model.Password = StringHelper.ToMD5(input.Password);
            }

            if (string.IsNullOrWhiteSpace(input.Avatar))
            {
                model.Avatar = entity.Avatar;
            }

            model.IsAdmin = entity.IsAdmin;
            model.CreationTime = entity.CreationTime;
            model.UpdateBy = LoginUser.UserName;
            model.UpdateTime = DateTime.Now;

            var res = await _userRepository.UpdateAsync(model);
            result.SetData(res);
            return result;
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ResultDto<bool>> Delete(int[] ids)
        {
            var result = new ResultDto<bool>();

            var list = await _userRepository.GetListAsync(p => ids.Contains(p.Id));
            var items = new List<User>();
            foreach (var item in list)
            {
                item.IsDelete = true;
                items.Add(item);
            }
            var b = await _userRepository.UpdateAsync(items);
            result.SetData(b);
            return result;
        }

        [HttpPost]
        [Route("set-role")]
        public async Task<ResultDto<bool>> AddUserRole([FromBody] UserRoleDto input)
        {
            var result = new ResultDto<bool>();

            var roles = string.Join(", ", input.RoleIds);

            var entity = _userRepository.GetQueryable(p => p.Id == input.Id).AsNoTracking().FirstOrDefault();
            if (entity != null)
            {
                entity.Roles = roles;
                var b = await _userRepository.UpdateAsync(entity);
                result.SetData(b);
            }

            return result;
        }

        [HttpGet]
        [Route("role-ids")]
        public async Task<ResultDto<int[]>> GetRoleIds(int userId)
        {
            var result = new ResultDto<int[]>();
            int[] ids = new int[] { };

            var user = await _userRepository.GetOneAsync(p => p.Id == userId && p.IsDelete == false);
            if (user.Roles != null)
            {
                ids = StringHelper.StrArrToIntArr(user.Roles.Split(','));
            }

            result.SetData(ids);
            return result;
        }

    }
}
