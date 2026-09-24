using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Campus.Domain.Interfaces;
using Campus.Domain.Entities;
using Campus.Domain.ValueObjects;
using Campus.Application.DTOs;

namespace Campus.Application.UseCases;

public class BuildingHandler
{
    private readonly IGenericRepository<Building> _repository;
    private readonly IGenericRepository<Campus.Domain.Entities.Campus> _campusRepository;

    public BuildingHandler(IGenericRepository<Building> repository, IGenericRepository<Campus.Domain.Entities.Campus> campusRepository)
    {
        _repository = repository;
        _campusRepository = campusRepository;
    }

    public async Task<IEnumerable<BuildingDto>> GetAllAsync(Guid? campusId, CancellationToken cancellationToken)
    {
        var buildings = await _repository.GetAllAsync(campusId.HasValue ? b => b.CampusId == campusId.Value : null, cancellationToken);
        return buildings.Select(b => new BuildingDto(b.Id, b.CampusId, b.Name, b.FloorsCount, b.Coordinates.Latitude, b.Coordinates.Longitude));
    }

    public async Task<BuildingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var building = await _repository.GetByIdAsync(id, cancellationToken);
        if (building == null) return null;
        return new BuildingDto(building.Id, building.CampusId, building.Name, building.FloorsCount, building.Coordinates.Latitude, building.Coordinates.Longitude);
    }

    public async Task<(BuildingDto? dto, string? error)> CreateAsync(CreateBuildingDto request, CancellationToken cancellationToken)
    {
        if (!await _campusRepository.ExistsAsync(c => c.Id == request.CampusId, cancellationToken))
            return (null, $"El campus con ID '{request.CampusId}' no existe.");

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        var building = new Building(request.Name, request.FloorsCount, coordinates, request.CampusId);
        
        await _repository.AddAsync(building, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return (new BuildingDto(building.Id, building.CampusId, building.Name, building.FloorsCount, building.Coordinates.Latitude, building.Coordinates.Longitude), null);
    }

    public async Task<(bool success, string? error)> UpdateAsync(Guid id, UpdateBuildingDto request, CancellationToken cancellationToken)
    {
        var building = await _repository.GetByIdAsync(id, cancellationToken);
        if (building == null) return (false, $"Edificio con ID '{id}' no fue encontrado.");

        if (!await _campusRepository.ExistsAsync(c => c.Id == request.CampusId, cancellationToken))
            return (false, $"El campus con ID '{request.CampusId}' no existe.");

        var coordinates = new Coordinate(request.Latitude, request.Longitude);
        building.Update(request.Name, request.FloorsCount, coordinates, request.CampusId);
        
        _repository.Update(building);
        await _repository.SaveChangesAsync(cancellationToken);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var building = await _repository.GetByIdAsync(id, cancellationToken);
        if (building == null) return false;

        _repository.Delete(building);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
