using System;
using System.Threading.Tasks;
using Domain.Models;
using Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMailService _mailService;

        public TestController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> TestSendMail([FromForm] MailRequestModel request)
        {
            try
            {
                await _mailService.SendAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}