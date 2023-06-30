using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPi.Data;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public readonly UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody]UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
               var user = await _userManager.FindByNameAsync(userDTO.Email);
                if (user != null)
                {
                    return BadRequest("用户名已存在，请重新选择用户名");
                }
                else
                {
                    IdentityUser identityUser = new IdentityUser()
                    {
                        UserName = userDTO.Email,
                        Email = userDTO.Email,
                        PhoneNumber = userDTO.Phone
                    };

                    identityUser.EmailConfirmed = true;
                    identityUser.SecurityStamp = DateTime.Now.Ticks.ToString();

                    var createResult = await _userManager.CreateAsync(identityUser, userDTO.PassWord);
                    if (createResult.Succeeded)
                    {
                        return Ok("用户创建成功");
                    }
                    else
                    {
                        return BadRequest("用户创建失败");
                    }
                }
            }

            return BadRequest("格式不正确，请输入正确的格式");
        }
    }
}
