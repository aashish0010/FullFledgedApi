using FullFledgedDto;
using FullFledgedInfrastructure;
using FullFledgedModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace FullFledgedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        //private readonly IJwtauthInterface _jwtauth;
        private readonly ApplicationDbContext _context;

        public UserRegisterController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("~/register")]
        [HttpPost]
        public IActionResult Register(UserLogin userLogin)
        {
            var viewPassword = new UserRegister();
            var user = _context.userRegisters.Where(x => x.UserName == userLogin.UserName).FirstOrDefault();
            if(user == null)
            {
                using (var hmc = new System.Security.Cryptography.HMACSHA512())
                {
                    viewPassword.PasswordHash = hmc.Key;
                    viewPassword.PasswordSalt = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLogin.Password));
                }
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
                    return Ok(message);
                }
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
