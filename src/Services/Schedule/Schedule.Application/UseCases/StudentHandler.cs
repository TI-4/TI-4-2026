using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedule.Application.DTOs;
using Schedule.Domain.Interfaces;

namespace Schedule.Application.UseCases;

public class StudentHandler
{
    private readonly IMeetingRepository _meetingRepository;

    public StudentHandler(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }

    public async Task<(StudentMeetingsDto? dto, string? error)> GetMeetingsAsync(
        Guid studentRefId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        if (to <= from)
        {
            return (null, "The 'to' parameter must be later than 'from'.");
        }

        var meetings = await _meetingRepository.GetByStudentAsync(
            studentRefId, from, to, cancellationToken);

        var dto = new StudentMeetingsDto(
            studentRefId,
            from,
            to,
            meetings.Count,
            meetings.Select(MeetingDto.FromEntity).ToList());

        return (dto, null);
    }
}

