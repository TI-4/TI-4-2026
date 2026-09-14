using System;

namespace Campus.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public Guid CampusId { get; set; }
    public Guid? BuildingId { get; set; }
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Campus Campus { get; set; } = null!;

    public Building? Building { get; set; }
    public Category Category { get; set; } = null!;
}
