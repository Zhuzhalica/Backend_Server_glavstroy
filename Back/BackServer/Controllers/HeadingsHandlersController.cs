using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackServer.Services.Interfaces;
using Entity;
using Microsoft.Extensions.Logging;

namespace BackServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeadingsHandlersController : ControllerBase
    {
        private readonly ILogger<HeadingsHandlersController> _logger;
        private readonly IHeadersService _service;

        public HeadingsHandlersController(ILogger<HeadingsHandlersController> logger, IHeadersService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("~/GetAllHeadingsOne")]
        public async Task<IEnumerable<Entity.HeadingOne>> GetAllHeadingsOne()
        {
            return await _service.GetAllHeadingsOne();
        }

        [HttpGet("~/GetAllHeadingsTwo")]
        public async Task<IEnumerable<Entity.HeadingTwo>> GetAllHeadingsTwo()
        {
            return await _service.GetAllHeadingsTwo();
        }
        
        [HttpGet("~/GetAllHeadingsThree")]
        public async Task<IEnumerable<Entity.HeadingThree>> GetAllHeadingsThree()
        {
            return await _service.GetAllHeadingsThree();
        }

        [HttpGet("~/GetHeadingsTwoByHeadingsOne")]
        public async Task<IEnumerable<Entity.HeadingTwo>> GetHeadingsTwoByHeadingsOne(string headingOneTitle)
        {
            return await _service.GetHeadingsTwoByHeadingsOne(headingOneTitle);
        }
        
        [HttpGet("~/GetHeadingsThreeByHeadingsTwo")]
        public async Task<IEnumerable<Entity.HeadingThree>> GetHeadingsThreeByHeadingsTwo(string headingTwoTitle)
        {
            return await _service.GetHeadingsThreeByHeadingsTwoAsync(headingTwoTitle);
        }

        [HttpPost("~/AddHeadingOne")]
        public async Task<StatusCodeResult> AddHeadingOne(HeadingOne headingOne)
        {
            var success = await _service.AddHeadingOne(headingOne);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/AddHeadingTwo")]
        public async Task<StatusCodeResult> AddHeadingTwo(HeadingTwo headingTwo)
        {
            var success = await _service.AddHeadingTwo(headingTwo);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/AddHeadingThree")]
        public async Task<StatusCodeResult> AddHeadingThree(HeadingThree headingThree)
        {
            var success = await _service.AddHeadingThree(headingThree);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("~/DeleteHeadingOne")]
        public async Task<StatusCodeResult> DeleteHeadingOne(string headingOneTitle)
        {
            var success = await _service.DeleteHeadingOne(headingOneTitle);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("~/DeleteHeadingTwo")]
        public async Task<StatusCodeResult> DeleteHeadingTwo(string headingTwoTitle)
        {
            var success = await _service.DeleteHeadingTwo(headingTwoTitle);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("~/DeleteHeadingThree")]
        public async Task<StatusCodeResult> DeleteHeadingThree(string headingThreeTitle)
        {
            var success = await _service.DeleteHeadingThree(headingThreeTitle);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/UpdateHeadingOne")]
        public async Task<StatusCodeResult> UpdateHeadingOne(string oldHeadingOneTitle, HeadingOne headingOne)
        {
            var success = await _service.UpdateHeadingOne(oldHeadingOneTitle, headingOne);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/UpdateHeadingTwo")]
        public async Task<StatusCodeResult> UpdateHeadingTwo(string oldHeadingTwoTitle, HeadingTwo headingTwo)
        {
            var success = await _service.UpdateHeadingTwo(oldHeadingTwoTitle, headingTwo);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/UpdateHeadingThree")]
        public async Task<StatusCodeResult> UpdateHeadingThree(string oldHeadingThreeTitle, HeadingThree headingThree)
        {
            var success = await _service.UpdateHeadingThree(oldHeadingThreeTitle, headingThree);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}