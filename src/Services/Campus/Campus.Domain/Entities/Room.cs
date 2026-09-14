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
    public string? Number { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Campus Campus { get; set; } = null!;

    public Building? Building { get; set; }
    public Category Category { get; set; } = null!;

    public Room(string name, int floor, string type, string number, double latitude, double longitude, Campus campus, Building building, Category category)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.Floor = floor;
        this.Type = type;
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
