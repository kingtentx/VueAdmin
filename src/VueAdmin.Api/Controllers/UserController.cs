using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        private IRepository<Admin> _userRepository;
        private JwtConfig _jwtSettings;

        public UserController(
            IMapper mapper,
            ICacheService cache,
            IOptions<JwtConfig> jwt,
            IRepository<Admin> userRepository

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
                    new Claim(ClaimTypes.Sid,user.AdminId.ToString()),
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
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("info")]
        public async Task<ResultDto> GetUserInfo()
        {
            var result = new ResultDto();

            var obj = new
            {
                roles = "admin",
                name = LoginUser.UserName,
                avatar = "https://cdn.mediecogroup.com/49/493822b4/493822b4085245bdb8b3625575df7ad3.jpg",
                introduction = "0000"
            };
            result.SetData(obj);

            return result;
        }
        //logout

        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<ResultDto> Logout()
        {
            var result = new ResultDto();

            result.SetData("success");

            return result;
        }

        [HttpPost]
        [Route("list")]
        public async Task<ResultDto> GetUsers(int pageIndex, int pageSize)
        {
            var result = new ResultDto();

            var list = _userRepository.GetList(p => true, pageIndex, pageSize);
            result.SetData(list);

            return result;
        }

    }
}
