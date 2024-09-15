using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Helper.SM4;
using VueAdmin.Repository;
using VueAdmin.Web.Areas.Dtos;
using System.Threading.Tasks;

namespace VueAdmin.Web.Areas.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer,Cookies")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class UserController : ApiBaseController
    {
        private ICacheService _cache;
        private IMapper _mapper;
        private IRepository<Admin> _userRepository;


        public UserController(
            IMapper mapper,
            ICacheService cache,
            IRepository<Admin> userRepository

            )
        {
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;

        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResultDto> TestSM4([FromBody] PlainTextDto text)
        {
            var result = new ResultDto();

            //string plainText = "123456789";
            string strKey = "mC7GSJxhyKtDjdUv";

            var sm4 = new SM4Utils();
            sm4.secretKey = strKey;
            sm4.hexString = false;
            var msg = sm4.Encrypt_ECB_ToBase64(text.PlainText);
            result.SetData(msg);
            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUserInfo()
        {
            //var user = LoginUser(User);
            var query = await _userRepository.GetOneAsync(p => p.UserName == LoginUser.UserName);

            if (query != null)
            {
                var data = _mapper.Map(query, new LoginUserDto());
                return Ok(data);
            }
            else
            {
                return BadRequest("用户不存在");
            }
        }




    }
}
