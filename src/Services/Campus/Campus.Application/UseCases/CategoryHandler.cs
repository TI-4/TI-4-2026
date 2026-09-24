using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Campus.Domain.Interfaces;
using Campus.Domain.Entities;
using Campus.Application.DTOs;

namespace Campus.Application.UseCases;

public class CategoryHandler
{
    private readonly IGenericRepository<Category> _repository;

    public CategoryHandler(IGenericRepository<Category> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(null, cancellationToken);
        return categories.Select(c => new CategoryDto(c.Id, c.Name, c.Icon, c.Description));
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);
        if (category == null) return null;
        return new CategoryDto(category.Id, category.Name, category.Icon, category.Description);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name, request.Icon, request.Description);
        await _repository.AddAsync(category, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return new CategoryDto(category.Id, category.Name, category.Icon, category.Description);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCategoryDto request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);
        if (category == null) return false;

        category.Update(request.Name, request.Icon, request.Description);
        _repository.Update(category);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);
        if (category == null) return false;

        _repository.Delete(category);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
