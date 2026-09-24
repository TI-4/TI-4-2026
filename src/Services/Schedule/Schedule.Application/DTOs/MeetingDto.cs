using System;
using Schedule.Domain.Entities;

namespace Schedule.Application.DTOs;

public record MeetingDto(
    Guid Id,
    Guid TeacherRefId,
    Guid StudentRefId,
    Guid StructureRefId,
    DateTime ScheduledAt,
    string Status)
{
    public static MeetingDto FromEntity(Meeting meeting) => new(
        meeting.Id,
        meeting.TeacherRefId,
        meeting.StudentRefId,
        meeting.StructureRefId,
        meeting.ScheduledAt,
        meeting.Status.ToString());
}
