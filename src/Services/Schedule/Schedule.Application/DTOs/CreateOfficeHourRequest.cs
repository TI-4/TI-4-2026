using System;

namespace Schedule.Application.DTOs;

public record CreateOfficeHourRequest(
    Guid TeacherRefId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime);
