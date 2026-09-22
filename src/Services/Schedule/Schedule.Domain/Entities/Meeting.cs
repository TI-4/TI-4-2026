using System;
using Schedule.Domain.Enums;

namespace Schedule.Domain.Entities;

public class Meeting
{
    public Guid Id { get; private set; }

    public Guid TeacherRefId { get; private set; }
    public Guid StudentRefId { get; private set; }
    public Guid StructureRefId { get; private set; }

    public DateTime ScheduledAt { get; private set; }
    public MeetingStatus Status { get; private set; }

    public Meeting(
        Guid teacherRefId,
        Guid studentRefId,
        Guid structureRefId,
        DateTime scheduledAt,
        OfficeHour? officeHour = null)
    {
        this.Id = Guid.NewGuid();
        this.TeacherRefId = teacherRefId;
        this.StudentRefId = studentRefId;
        this.StructureRefId = structureRefId;
        this.ScheduledAt = scheduledAt;
        this.Status = MeetingStatus.Pending;

        this.OfficeHour = officeHour;
        this.OfficeHourId = officeHour?.Id;
    }

    protected Meeting() { }
}
