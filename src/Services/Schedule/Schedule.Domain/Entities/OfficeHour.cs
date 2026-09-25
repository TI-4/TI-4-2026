using System;

namespace Schedule.Domain.Entities;

public class OfficeHour
{
    public Guid Id { get; private set; }

    public Guid TeacherRefId { get; private set; }
    public Guid StructureRefId { get; private set; }

    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public OfficeHour(
        Guid teacherRefId,
        Guid structureRefId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            throw new ArgumentException(
                "End Hour must be later than Start Hour", nameof(endTime));
        }

        this.Id = Guid.NewGuid();
        this.TeacherRefId = teacherRefId;
        this.StructureRefId = structureRefId;
        this.DayOfWeek = dayOfWeek;
        this.StartTime = startTime;
        this.EndTime = endTime;
    }

    protected OfficeHour() { }
}
