using ErrorOr;
using Incident.Application.DTOs;
using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using System.Threading.Tasks;
using System;

namespace Incident.Application.Handlers;

public class LostObjectHandler : ILostObjectHandler
{
    private readonly ILostObjectRepository _lostObjectRepository;

    public LostObjectHandler(ILostObjectRepository lostObjectRepository)
    {
        _lostObjectRepository = lostObjectRepository;
    }

    public async Task<ErrorOr<string>> CreateObjectAsync(CreateLostObjectRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.PhotoUrl))
        {
            if (!Uri.TryCreate(request.PhotoUrl, UriKind.Absolute, out var uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return Error.Validation("LostObject.PhotoUrl", "Invalid Photo URL format.");
            }
        }

        if (request.StructureId == Guid.Empty)
        {
            return Error.Validation("LostObject.StructureId", "Structure ID is required.");
        }

        var lostObject = new LostObject
        {
            Title = request.Title,
            Description = request.Description,
            Status = (Objectenum)request.Status,
            PhotoUrl = request.PhotoUrl,
            StructureRefId = request.StructureId
        };

        await _lostObjectRepository.CreateAsync(lostObject);

        return lostObject.Id ?? string.Empty;
    }
}

