using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Campus.Domain.Entities;
using Campus.Application.DTOs;
using Campus.Infraestructure.Persistence;

namespace Campus.API.Endpoints;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly CampusDbContext _context;

    public CategoryController(CampusDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Icon,
                c.Description
            ))
            .ToListAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound(new { message = $"Categoría con ID '{id}' no fue encontrada." });
        }

        var dto = new CategoryDto(
            category.Id,
            category.Name,
            category.Icon,
            category.Description
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre de la categoría es obligatorio." });
        }

        var category = new Category(request.Name, request.Icon, request.Description);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        var responseDto = new CategoryDto(
            category.Id,
            category.Name,
            category.Icon,
            category.Description
        );

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, responseDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "El nombre de la categoría es obligatorio." });
        }

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound(new { message = $"Categoría con ID '{id}' no fue encontrada." });
        }

        category.Update(request.Name, request.Icon, request.Description);

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound(new { message = $"Categoría con ID '{id}' no fue encontrada." });
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
