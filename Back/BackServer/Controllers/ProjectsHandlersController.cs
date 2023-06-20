using System;
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
    public class ProjectsHandlersController : ControllerBase
    {
        private readonly ILogger<ProjectsHandlersController> _logger;
        private readonly IProjectService _service;

        public ProjectsHandlersController(ILogger<ProjectsHandlersController> logger, IProjectService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("~/GetAllProjects")]
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _service.GetAll();
        }

        [HttpGet("~/GetRangeProjects")]
        public async Task<IEnumerable<Project>> GetRangeProjects(int pageNumber, int countElements)
        {
            return await _service.GetRange(pageNumber, countElements);
        }


        [HttpGet("~/GetProductsByProject")]
        public async Task<IEnumerable<Product>> GetProductsByProject(string projectTitle)
        {
            return await _service.GetProductByProject(projectTitle);
        }
        
        [HttpGet("~/GetProjectByProduct")]
        public async Task<IEnumerable<Project>> GetProjectByProduct(string productTitle)
        {
            return await _service.GetProjectByProduct(productTitle);
        }


        [HttpPost("~/AddProject")]
        public async Task<StatusCodeResult> AddProject(Project project)
        {
            var success = await _service.Add(project);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("~/DeleteProject")]
        public async Task<StatusCodeResult> DeleteProject(string projectTitle)
        {
            var success = await _service.Delete(projectTitle);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/UpdateProject")]
        public async Task<StatusCodeResult> UpdateProject(string oldProjectTitle, Project project)
        {
            var success = await _service.Update(oldProjectTitle, project);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPost("~/AddProductsProject")]
        public async Task<StatusCodeResult> AddProducts(string projectTitle, HashSet<string> productTitles)
        {
            var success = await _service.AddProducts(projectTitle, productTitles);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("~/DeleteProductsProject")]
        public async Task<StatusCodeResult> DeleteProducts(string projectTitle, HashSet<string> productTitles)
        {
            var success = await _service.DeleteProducts(projectTitle, productTitles);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("~/DeleteAllProductsProject")]
        public async Task<StatusCodeResult> DeleteAllProduct(string projectTitle)
        {
            var success = await _service.DeleteAllProduct(projectTitle);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}