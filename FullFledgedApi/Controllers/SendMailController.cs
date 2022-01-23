using FullFledgedModel;
using FullFledgedRepository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FullFledgedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly IMailServiceInterface _mailServiceInterface;

        public SendMailController(IMailServiceInterface mailServiceInterface)
        {
            _mailServiceInterface = mailServiceInterface;
        }
        [Route("~/SendMail")]
        [HttpGet]
        public async Task<IActionResult> Send([FromBody] SendMail sendMail)
        {
            try
            {
                await _mailServiceInterface.SendEmailAsync(sendMail);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
