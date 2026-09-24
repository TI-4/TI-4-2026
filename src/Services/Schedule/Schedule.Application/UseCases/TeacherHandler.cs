using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedule.Application.DTOs;
using Schedule.Domain.Interfaces;

namespace Schedule.Application.UseCases;

public class TeacherHandler
{
    private readonly IOfficeHourRepository _officeHourRepository;
    private readonly IMeetingRepository _meetingRepository;

    public TeacherHandler(
        IOfficeHourRepository officeHourRepository,
        IMeetingRepository meetingRepository)
    {
        _officeHourRepository = officeHourRepository;
        _meetingRepository = meetingRepository;
    }

    public async Task<(TeacherWorkloadDto? dto, string? error)> GetWorkloadAsync(
        Guid teacherRefId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        if (to <= from)
        {
            return (null, "The 'to' parameter must be later than 'from'.");
        }

        var officeHours = await _officeHourRepository.GetByTeacherAsync(teacherRefId, cancellationToken);
        var meetings = await _meetingRepository.GetByTeacherAsync(teacherRefId, from, to, cancellationToken);

        var weeklyOfficeHourMinutes = officeHours
            .Sum(officeHour => (int)(officeHour.EndTime - officeHour.StartTime).TotalMinutes);

        var dto = new TeacherWorkloadDto(
            teacherRefId,
            from,
            to,
            weeklyOfficeHourMinutes,
            meetings.Count,
            officeHours.Select(OfficeHourDto.FromEntity).ToList(),
            meetings.Select(MeetingDto.FromEntity).ToList());

        return (dto, null);
    }

    public async Task<TeacherAvailabilityDto> GetAvailabilityAsync(
        Guid teacherRefId,
        CancellationToken cancellationToken)
    {
        var officeHours = await _officeHourRepository.GetByTeacherAsync(teacherRefId, cancellationToken);

        return new TeacherAvailabilityDto(
            teacherRefId,
            officeHours.Select(OfficeHourDto.FromEntity).ToList());
    }
}
