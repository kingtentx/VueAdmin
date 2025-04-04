using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VueAdmin.Api.Dtos;
using VueAdmin.Api.Dtos.User;
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
        private JwtConfig _jwtSettings;

        public UserController(
            IMapper mapper,
            ICacheService cache,
            IOptions<JwtConfig> jwt,
            IRepository<User> userRepository

            )
        {
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtSettings = jwt.Value;
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
                var user = await _userRepository.GetOneAsync(p => p.UserName == input.UserName && p.Password == StringHelper.ToMD5(input.Password));
                if (user == null)
                {
                    result.Msg = "用户启或密码错误";
                    return result;
                }

                var claim = new Claim[]{
                    new Claim(ClaimTypes.Sid,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName)
                };

                //对称秘钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                //签名证书(秘钥，加密算法)
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claim, DateTime.Now, DateTime.Now.AddMinutes(_jwtSettings.Expiration), creds);

                var obj = new UserInfoDto
                {
                    UserName = user.UserName,
                    NickName = user.NickName,
                    Avatar = user.Avatar,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = "",
                    Roles = new List<string> { "admin" },
                    Permissions = new List<string> { "*:*:*" },
                    Expires = DateTime.Now.AddMinutes(_jwtSettings.Expiration)
                };
                result.SetData(obj);
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
            if (input.Status != null)
            {
                where = where.And(p => p.IsActive == input.Status);
            }

            var items = await _userRepository.GetListAsync(where, p => p.Id, input.CurrentPage, input.PageSize);

            var data = new ItemList<UserDto>
            {
                Total = items.Count,
                List = _mapper.Map<List<UserDto>>(items.List),
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
            var query = await _userRepository.GetOneAsync(p => p.UserName.Equals(input.UserName));
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
            var query = await _userRepository.GetOneAsync(p => p.UserName.Equals(input.UserName) && p.Id != input.Id);
            if (query != null)
            {
                result.Msg = "用户名已存在";
                return result;
            }

            var entity = _userRepository.GetQueryable(p => p.Id == input.Id).AsNoTracking().FirstOrDefault();
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
            var users = new List<User>();
            foreach (var item in list)
            {
                item.IsDelete = true;
                users.Add(item);
            }
            var b = await _userRepository.UpdateAsync(users);
            result.SetData(b);
            return result;
        }

    }
}
