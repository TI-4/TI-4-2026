using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedule.Application.DTOs;
using Schedule.Domain.Interfaces;

namespace Schedule.Application.UseCases;

public class TeacherHandler(
    IOfficeHourRepository officeHourRepository,
    IMeetingRepository meetingRepository)
{
    public async Task<TeacherWorkloadDto> GetWorkloadAsync(
        Guid teacherRefId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default)
    {
        var officeHours = await officeHourRepository.GetByTeacherAsync(teacherRefId, cancellationToken);
        var meetings = await meetingRepository.GetByTeacherAsync(teacherRefId, from, to, cancellationToken);

        var weeklyOfficeHourMinutes = officeHours
            .Sum(officeHour => (int)(officeHour.EndTime - officeHour.StartTime).TotalMinutes);

        return new TeacherWorkloadDto(
            teacherRefId,
            from,
            to,
            weeklyOfficeHourMinutes,
            meetings.Count,
            officeHours.Select(OfficeHourDto.FromEntity).ToList(),
            meetings.Select(MeetingDto.FromEntity).ToList());
    }
}
