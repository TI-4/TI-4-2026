using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedule.Application.DTOs;
using Schedule.Domain.Entities;
using Schedule.Domain.Interfaces;

namespace Schedule.Application.UseCases;

public class MeetingHandler
{
    private readonly IMeetingRepository _repository;

    public MeetingHandler(IMeetingRepository repository)
    {
        _repository = repository;
    }

    public async Task<MeetingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var meeting = await _repository.GetByIdAsync(id, cancellationToken);

        return meeting is null ? null : MeetingDto.FromEntity(meeting);
    }

    public async Task<(IEnumerable<MeetingDto>? dtos, string? error)> GetByTeacherAsync(
        Guid teacherRefId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        if (to <= from)
        {
            return (null, "The 'to' parameter must be later than 'from'.");
        }

        var meetings = await _repository.GetByTeacherAsync(teacherRefId, from, to, cancellationToken);

        return (meetings.Select(MeetingDto.FromEntity), null);
    }

    public async Task<(MeetingDto? dto, string? error)> CreateAsync(
        CreateMeetingRequest request,
        CancellationToken cancellationToken)
    {
        var meeting = new Meeting(
            request.TeacherRefId,
            request.StudentRefId,
            request.StructureRefId,
            request.ScheduledAt);

        await _repository.AddAsync(meeting, cancellationToken);

        return (MeetingDto.FromEntity(meeting), null);
    }
}
