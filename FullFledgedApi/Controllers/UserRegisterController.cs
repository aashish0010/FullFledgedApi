using FullFledgedDto;
using FullFledgedInfrastructure;
using FullFledgedModel;
using FullFledgedRepository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace FullFledgedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        //private readonly IJwtauthInterface _jwtauth;
        private readonly ApplicationDbContext _context;
        private readonly ITokenInterface _tokenInterface;
        private readonly IConfiguration _config;

        public UserRegisterController(ApplicationDbContext context, ITokenInterface tokenInterface,IConfiguration config)
        {
            _context = context;
            _tokenInterface = tokenInterface;
            _config = config;
        }
        [Route("~/register")]
        [HttpPost]
        public IActionResult Register(UserLogin userLogin)
        {
            var token = _tokenInterface.TokenGenerateString(userLogin);
            var viewPassword = new UserRegister();
            
            var user = _context.userRegisters.FirstOrDefault(x=>x.UserName == userLogin.UserName);
            if(user == null)
            {
                using (var hmc = new System.Security.Cryptography.HMACSHA512())
                {
                    viewPassword.PasswordHash = hmc.Key;
                    viewPassword.PasswordSalt = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLogin.Password));
                }
                viewPassword.Token = token;
                viewPassword.Email = userLogin.Email;
                viewPassword.UserName = userLogin.UserName;
                viewPassword.Created = DateTime.Now;
                viewPassword.UpdateDate = DateTime.Now;
                viewPassword.Status = 1;
                _context.userRegisters.Add(viewPassword);
                _context.SaveChanges();
            }
            
            
            return Ok(viewPassword);
        }
        [Route("~/login")]
        [HttpPost]
        public IActionResult Login(UserLoginResult userLoginResult)
        {
            

            List<Message> message = new List<Message>();
            var messagedata = new Message();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                
                var principal = tokenHandler.ValidateToken(userLoginResult.Token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey=true,
                    ValidateLifetime = true, 
                    ValidateAudience = false, // Because there is no audiance in the generated token
                    ValidateIssuer = false,   // Because there is no issuer in the generated token
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
                }, out validatedToken);
                
                if(principal != null)
                {

                    var user = _context.userRegisters.Where(x => x.UserName == userLoginResult.UserName).FirstOrDefault();
                    using (var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordHash))
                    {
                        var compute = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLoginResult.Password));
                        bool message12 = compute.SequenceEqual(user.PasswordSalt);
                        if (message12 == true)
                        {
                            messagedata.StatusCode = 0;
                            messagedata.StatusMessage = "login Successfully.";
                            message.Add(messagedata);
                        }
                        else
                        {
                            messagedata.StatusCode = 1;
                            messagedata.StatusMessage = "Password incorrect.";
                            message.Add(messagedata);
                        }
                       
                    }
                }
                return Ok(message);

            }
            catch (Exception ex)
            {
                messagedata.StatusCode = 1;
                messagedata.StatusMessage = ex.Message;
                message.Add(messagedata);
                return BadRequest(message);
                
            }
            
                

        }
    }
}
