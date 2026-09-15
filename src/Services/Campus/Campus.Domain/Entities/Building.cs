using System;
using System.Collections.Generic;

namespace Campus.Domain.Entities;

public class Building
{
    public Guid Id { get; private set; }
    public Guid CampusId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int FloorsCount { get; private set; } 
    
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public Campus Campus { get; private set; } = null!;
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    public Building(string name, int floorsCount, double latitude, double longitude, Campus campus)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.FloorsCount = floorsCount;
        this.Latitude = latitude;
        this.Longitude = longitude;
        
        this.Campus = campus;
        this.CampusId = campus.Id;
    }

    protected Building() { }
}
