using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedule.Application.DTOs;
using Schedule.Domain.Entities;
using Schedule.Domain.Interfaces;

namespace Schedule.Application.UseCases;

public class OfficeHourHandler
{
    private readonly IOfficeHourRepository _repository;

    public OfficeHourHandler(IOfficeHourRepository repository)
    {
        _repository = repository;
    }

    public async Task<OfficeHourDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var officeHour = await _repository.GetByIdAsync(id, cancellationToken);

        return officeHour is null ? null : OfficeHourDto.FromEntity(officeHour);
    }

    public async Task<IEnumerable<OfficeHourDto>> GetByTeacherAsync(
        Guid teacherRefId,
        CancellationToken cancellationToken)
    {
        var officeHours = await _repository.GetByTeacherAsync(teacherRefId, cancellationToken);

        return officeHours.Select(OfficeHourDto.FromEntity);
    }

    public async Task<(OfficeHourDto? dto, string? error)> CreateAsync(
        CreateOfficeHourRequest request,
        CancellationToken cancellationToken)
    {
        OfficeHour officeHour;

        try
        {
            officeHour = new OfficeHour(
                request.TeacherRefId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);
        }
        catch (ArgumentException ex)
        {
            return (null, ex.Message);
        }

        await _repository.AddAsync(officeHour, cancellationToken);

        return (OfficeHourDto.FromEntity(officeHour), null);
    }
}
