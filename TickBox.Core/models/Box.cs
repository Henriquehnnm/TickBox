namespace TickBox.Core.Models;

public abstract class Box
{
    public Guid Id;
    protected string Name;
    protected string Description;
    protected bool Archived;
    protected DateTime CreatedAt;
    protected DateTime UpdatedAt;

    protected Box(string name, string description)
    {
        DateTime now = DateTime.Now;
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Archived = false;
        CreatedAt = now;
        UpdatedAt = now;
    }
}
