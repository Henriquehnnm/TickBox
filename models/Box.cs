namespace TickBox.Core.Models;

public abstract class Box
{
    public Guid Id;
    public string Name;
    public string Description;
    public bool Archived;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;

    public Box(string name, string description)
    {
        DateTime now = DateTime.Now;
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.Description = description;
        this.Archived = false;
        this.CreatedAt = now;
        this.UpdatedAt = now;
    }
}
