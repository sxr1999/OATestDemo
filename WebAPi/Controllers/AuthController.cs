using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPi.Data;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOptions<JwtDto> _options; 
        

        public AuthController(SignInManager<IdentityUser> signInManager, IOptions<JwtDto> options)
        {
            _signInManager = signInManager;
            _options = options;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var loginResult =
                    await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, true, false);
                if (loginResult.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, loginDto.Email),
                        new Claim(ClaimTypes.Email, loginDto.Email)
                    };
                    
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecurityKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var securityToken = new JwtSecurityToken(
                        issuer: _options.Value.Issuer,
                        audience: _options.Value.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: creds);

                    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                    
                    return Ok(new
                    {
                        token = token,
                        Message = "登录成功"
                    });
                }

                return BadRequest("登录失败，请检查用户名和密码是否正确");
            }

            return BadRequest("输入格式错误");
        }
        
    }
}
