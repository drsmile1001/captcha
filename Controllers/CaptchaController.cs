using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Captcha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptchaController : ControllerBase
    {
        private readonly ILogger<CaptchaController> _logger;
        private readonly CapatchaService _capatchaService;

        public CaptchaController(ILogger<CaptchaController> logger, CapatchaService capatchaService)
        {
            _logger = logger;
            _capatchaService = capatchaService;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var (id, image) = await _capatchaService.GenerateRecordAsync();

            return Ok(new
            {
                Id = id,
                Image = image
            });
        }

        [HttpPost("{id}/Match/{text}")]
        public async Task<ActionResult<bool>> Match(int id, string text)
        {
            var match = await _capatchaService.MatchRecord(id,text);
            return Ok(match);
        }
    }
}
