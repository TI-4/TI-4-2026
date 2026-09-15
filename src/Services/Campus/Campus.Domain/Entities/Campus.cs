using System;
using System.Collections.Generic;

namespace Campus.Domain.Entities;

public class Campus
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public ICollection<Building> Buildings { get; private set; } = new List<Building>();
    public ICollection<Room> StandAloneRooms { get; private set; } = new List<Room>();

    public Campus(string name, string address, double latitude, double longitude)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.Address = address;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

    protected Campus() { }
}
