using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHangFire.Jobs;
using OrchardCore.Users.Services;

namespace OCHangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService _userService;

        public TestController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            // NOTE:  We can see that, the service is able to resolve _userService
            var user = await _userService.GetUserAsync("admin");
            string userName = user?.UserName;
            return Ok(new
            {
                UserName = userName
            });
        }
        [HttpGet]
        [Route("new-job")]
        public IActionResult NewJob()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            BackgroundJob.Enqueue<MyBackgroundJob>(x => x.ExecuteAsync(now));
            return Ok($"New Job: {now}");
        }
    }
}
