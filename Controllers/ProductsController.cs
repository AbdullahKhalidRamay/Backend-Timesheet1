using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeSheetAPI.Models;
using TimeSheetAPI.Models.DTOs;
using TimeSheetAPI.Services;

namespace TimeSheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProductsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            // For now, we'll treat products as a special type of project
            var projects = await _projectService.GetAllProjectsAsync();
            var productDtos = projects.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsBillable = p.IsBillable,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,
                Stages = new List<ProductStageDto>() // Convert project levels to product stages
            }).ToList();
            
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            var productDto = new ProductDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                IsBillable = project.IsBillable,
                CreatedBy = project.CreatedBy,
                CreatedAt = project.CreatedAt,
                Stages = new List<ProductStageDto>()
            };

            return Ok(productDto);
        }

        [HttpPost]
        [Authorize(Roles = "owner,manager")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                IsBillable = request.IsBillable,
                Status = "active",
                CreatedBy = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString()),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdProject = await _projectService.CreateProjectAsync(project);
            
            var productDto = new ProductDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                Description = createdProject.Description,
                IsBillable = createdProject.IsBillable,
                CreatedBy = createdProject.CreatedBy,
                CreatedAt = createdProject.CreatedAt,
                Stages = new List<ProductStageDto>()
            };

            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "owner,manager")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            project.Name = request.Name ?? project.Name;
            project.Description = request.Description ?? project.Description;
            project.IsBillable = request.IsBillable ?? project.IsBillable;
            project.UpdatedAt = DateTime.UtcNow;

            await _projectService.UpdateProjectAsync(project);

            var productDto = new ProductDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                IsBillable = project.IsBillable,
                CreatedBy = project.CreatedBy,
                CreatedAt = project.CreatedAt,
                Stages = new List<ProductStageDto>()
            };

            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "owner")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductStageDto> Stages { get; set; } = new List<ProductStageDto>();
    }

    public class ProductStageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProductTaskDto> Tasks { get; set; } = new List<ProductTaskDto>();
    }

    public class ProductTaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductSubtaskDto> Subtasks { get; set; } = new List<ProductSubtaskDto>();
    }

    public class ProductSubtaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; } = false;
    }

    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsBillable { get; set; }
    }
}
