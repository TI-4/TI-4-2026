using System;
using System.Collections.Generic;

namespace Schedule.Application.DTOs;

public record StudentScheduleDto(
    Guid StudentRefId,
    DateTime From,
    DateTime To,
    int MeetingCount,
    IReadOnlyList<MeetingDto> Meetings);
