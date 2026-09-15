using System;
using System.Collections.Generic;

namespace Campus.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public ICollection<Room> Rooms { get; private set; } = new List<Room>();
    public ICollection<Structure> Structures { get; private set; } = new List<Structure>();

    public Category(string name, string icon, string description)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.Icon = icon;
        this.Description = description;
    }

    protected Category() { }
}
