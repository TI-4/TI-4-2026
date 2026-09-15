using System;

namespace Campus.Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public Guid CampusId { get; private set; }
    public Guid? BuildingId { get; private set; }
    public Guid CategoryId { get; private set; }
    
    public string Name { get; private set; } = string.Empty;
    public int Floor { get; private set; }
    public string? Number { get; private set; }
    
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public Campus Campus { get; private set; } = null!;

    public Building? Building { get; private set; }
    public Category Category { get; private set; } = null!;

    public Room(string name, int floor, string? number, double latitude, double longitude, Campus campus, Building? building, Category category)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.Floor = floor;
        this.Number = number;
        this.Latitude = latitude;
        this.Longitude = longitude;
        
        this.Campus = campus;
        this.CampusId = campus.Id;
        
        this.Building = building;
        this.BuildingId = building?.Id;
        
        this.Category = category;
        this.CategoryId = category.Id;
    }

    protected Room() { }
}
