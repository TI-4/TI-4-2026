using ErrorOr;
using Incident.Application.DTOs;
using Incident.Domain.Entities;
using Incident.Domain.Repositories;
using System.Threading.Tasks;
using System;


namespace Incident.Application.Handlers;

public class ObjectHandler
{
    private readonly ILostObjectRepository _ObjectRepository;

    public ObjectHandler(ILostObjectRepository ObjectRepository)
    {
        _ObjectRepository = ObjectRepository;
    }

    public async Task<ErrorOr<LostObject>> CreateObjectAsync(CreateObjectRequest request)
    {
        if (!Uri.TryCreate(request.PhotoUrl, UriKind.Absolute, out var uriResult) ||
                    (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            return Error.Validation("Report.PhotoUrl", "PhotoUrl Invalidates.");
        }
        LostObject lostObject;
        try
        {
            lostObject = new LostObject
            {
                title = request.Title,
                description = request.Description,
                status = (Objectenum)request.Status,
                photo_url = request.PhotoUrl
            };
        }
        catch (ArgumentException ex)
        {
            return Error.NotFound(code: "NotFound:LostObject", description: $"Request error '{ex}'");
        }
        await _ObjectRepository.CreateAsync(lostObject);

        return lostObject;
    }
}
