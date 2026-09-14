using System;
using System.Collections.Generic;

namespace Campus.Domain.Entities;

public class Campus
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    
    public ICollection<Building> Buildings { get; set; } = new List<Building>();
    public ICollection<Room> StandAloneRooms { get; set; } = new List<Room>();
}
