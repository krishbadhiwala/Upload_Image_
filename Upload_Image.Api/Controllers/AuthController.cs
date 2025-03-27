//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Upload_Image.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
      
       
//            [HttpPost("login")]
//            public IActionResult Login([FromBody] LoginRequest request)
//            {
//                if (request.Username == "string" && request.Password == "123")
//                {
//                    var tokenHandler = new JwtSecurityTokenHandler();
//                    var key = Encoding.UTF8.GetBytes("Krish_Bdhiwala_Signinkey");
//                    var tokenDescriptor = new SecurityTokenDescriptor
//                    {
//                        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, request.Username) }),
//                        Expires = DateTime.UtcNow.AddDays(1),
//                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//                    };

//                    var token = tokenHandler.CreateToken(tokenDescriptor);
//                    return Ok(new { Token = tokenHandler.WriteToken(token) });
//                }
//                return Unauthorized();
//            }
//        }

//        public class LoginRequest
//        {
//            public string Username { get; set; }
//            public string Password { get; set; }
//        }

//    }

