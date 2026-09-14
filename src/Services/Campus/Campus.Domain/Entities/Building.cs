using System;
using System.Collections.Generic;

namespace Campus.Domain.Entities;

public class Building
{
    public Guid Id { get; set; }
    public Guid CampusId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int FloorsCount { get; set; } 
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Campus Campus { get; set; } = null!;
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
