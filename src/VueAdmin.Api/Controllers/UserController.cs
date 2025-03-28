using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
                    Username = user.UserName,
                    Nickname = user.RealName,
                    Avatar = "",
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
            var result = new ResultDto<ItemList<UserDto>>();

            var where = LambdaHelper.True<User>().And(p => true);
            if (!string.IsNullOrEmpty(input.UserAccount))
            {
                where = where.And(p => p.UserName.Contains(input.UserAccount));
            }
            if (!string.IsNullOrEmpty(input.UserName))
            {
                where = where.And(p => p.RealName.Contains(input.UserName));
            }
            if (input.IsActive != null)
            {
                where = where.And(p => p.IsActive == input.IsActive);
            }

            var items = await _userRepository.GetListAsync(where, p => p.Id, input.Page, input.PageSize);

            var data = new ItemList<UserDto>
            {
                Count = items.Count,
                Items = _mapper.Map<List<UserDto>>(items.List)
            };

            result.SetData(data);
            return result;
        }

        [HttpPost]
        [Route("add")]
        public async Task<ResultDto<bool>> Create([FromBody] CreateUpdateUserDto input)
        {
            var result = new ResultDto<bool>();
            var model = _mapper.Map<User>(input);
            model.Password = StringHelper.ToMD5("123qwe");

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

            var model = _mapper.Map<User>(input);
            model.Password = entity.Password;
            model.CreationTime = entity.CreationTime;
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
            await _userRepository.UpdateAsync(users);

            return result;
        }

    }
}
