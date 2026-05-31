using System.Text.Json;

namespace TickBox.Core.Models;

public record ParentData(
    Guid Id,
    string Name,
    string Description,
    bool Archived,
    List<String> ChildrenName,
    bool IsDraft,
    DateTime StarDate,
    DateTime EndDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

enum CreateType
{
    User,
    Object
}

public class ParentBox : Box
{
    public List<String> ChildrenName = [];
    public bool IsDraft;
    public DateTime StartDate;
    public DateTime EndDate;

    public ParentBox(string name, string description, DateTime startDate, DateTime endDate) : base(name, description)
    {
        this.StartDate = startDate;
        this.EndDate = endDate;
    }


    public void ToggleDraft()
    {
        this.IsDraft = !this.IsDraft;
    }

    public void AddChild(ChildBox child)
    {
        if (IsDraft)
        {
            ToggleDraft();
        }


        ChildrenName.Add(child.Name);
    }

    public async Task CreateAsync(string path, string filePath)
    {
        var option = new JsonSerializerOptions
        {
            WriteIndented = true
        }; // TODO - Criar cache pra isso, para extrair performance.
        if (!Directory.Exists(path))
        {
            await Task.Run(() => Directory.CreateDirectory(path));
        }

        var meta = Export();
        string json = JsonSerializer.Serialize(meta, option);
        await File.WriteAllTextAsync(filePath, json);
    }

    public ParentData Export()
    {
        return new ParentData(this.Id, this.Name, this.Description, this.Archived, this.ChildrenName, this.IsDraft,
            this.StartDate, this.EndDate, this.CreatedAt, this.UpdatedAt);
    }
}
