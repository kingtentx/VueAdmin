using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VueAdmin.Api.Dtos;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VueAdmin.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private JwtConfig _jwtSettings;
        private ICacheService _cache;
        private IRepository<Admin> _userRepository;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="cache"></param>
        /// <param name="userRepository"></param>
        public AuthorizeController(IOptions<JwtConfig> jwt, ICacheService cache, IRepository<Admin> userRepository)
        {
            _jwtSettings = jwt.Value;
            _cache = cache;
            _userRepository = userRepository;
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserInputDto model)
        {
            model.ValidateKey = "";
            model.ValidateValue = "";
            //if (model.ValidateKey == "" && model.ValidateValue == "")
            //{
            //    return BadRequest("参数错误");
            //}

            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))//判断账号密码是否正确
            {
                var user = await _userRepository.GetOneAsync(p => p.UserName == model.UserName && p.Password == StringHelper.ToMD5(model.Password));
                if (user == null)
                    return BadRequest("用户名密码错误");


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

                return Ok(new { user.UserName, Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest("参数错误");
        }

    }
}