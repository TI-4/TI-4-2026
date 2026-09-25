using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Campus.Domain.Interfaces;
using Campus.Domain.Entities;
using Campus.Application.DTOs;

namespace Campus.Application.UseCases;

public class RoomHandler
{
    private readonly IGenericRepository<Room> _repository;
    private readonly IGenericRepository<Building> _buildingRepository;
    private readonly IGenericRepository<Category> _categoryRepository;

    public RoomHandler(IGenericRepository<Room> repository, IGenericRepository<Building> buildingRepository, IGenericRepository<Category> categoryRepository)
    {
        _repository = repository;
        _buildingRepository = buildingRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync(Guid? buildingId, Guid? categoryId, CancellationToken cancellationToken)
    {
        var rooms = await _repository.GetAllAsync(r => (!buildingId.HasValue || r.BuildingId == buildingId.Value) && (!categoryId.HasValue || r.CategoryId == categoryId.Value), cancellationToken);
        return rooms.Select(r => new RoomDto(r.Id, r.BuildingId, r.CategoryId, r.Name, r.Floor, r.Number));
    }

    public async Task<IEnumerable<RoomDto>> SearchRoomsAsync(string term, CancellationToken cancellationToken)
    {
        var rooms = await _repository.GetAllAsync(r => r.Name.Contains(term) || (r.Number != null && r.Number.Contains(term)), cancellationToken);
        return rooms.Select(r => new RoomDto(r.Id, r.BuildingId, r.CategoryId, r.Name, r.Floor, r.Number));
    }

    public async Task<IEnumerable<RoomDto>> GetRoomsByBuildingAsync(Guid buildingId, CancellationToken cancellationToken)
    {
        var rooms = await _repository.GetAllAsync(r => r.BuildingId == buildingId, cancellationToken);
        return rooms.Select(r => new RoomDto(r.Id, r.BuildingId, r.CategoryId, r.Name, r.Floor, r.Number));
    }

    public async Task<RoomDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(id, cancellationToken);
        if (room == null) return null;
        return new RoomDto(room.Id, room.BuildingId, room.CategoryId, room.Name, room.Floor, room.Number);
    }

    public async Task<(RoomDto? dto, string? error)> CreateAsync(CreateRoomDto request, CancellationToken cancellationToken)
    {
        if (!await _buildingRepository.ExistsAsync(b => b.Id == request.BuildingId, cancellationToken))
            return (null, $"El edificio con ID '{request.BuildingId}' no existe.");
            
        if (!await _categoryRepository.ExistsAsync(c => c.Id == request.CategoryId, cancellationToken))
            return (null, $"La categoría con ID '{request.CategoryId}' no existe.");

        var room = new Room(request.Name, request.Floor, request.Number, request.BuildingId, request.CategoryId);
        
        await _repository.AddAsync(room, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return (new RoomDto(room.Id, room.BuildingId, room.CategoryId, room.Name, room.Floor, room.Number), null);
    }

    public async Task<(bool success, string? error)> UpdateAsync(Guid id, UpdateRoomDto request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(id, cancellationToken);
        if (room == null) return (false, null);

        if (!await _buildingRepository.ExistsAsync(b => b.Id == request.BuildingId, cancellationToken))
            return (false, $"El edificio con ID '{request.BuildingId}' no existe.");
            
        if (!await _categoryRepository.ExistsAsync(c => c.Id == request.CategoryId, cancellationToken))
            return (false, $"La categoría con ID '{request.CategoryId}' no existe.");

        room.Update(request.Name, request.Floor, request.Number, request.BuildingId, request.CategoryId);
        
        _repository.Update(room);
        await _repository.SaveChangesAsync(cancellationToken);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(id, cancellationToken);
        if (room == null) return false;

        _repository.Delete(room);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
