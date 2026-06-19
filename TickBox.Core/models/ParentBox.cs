using System.Text.Json;

namespace TickBox.Core.Models;

public record ParentData(
    Guid Id,
    string Name,
    string Description,
    bool Archived,
    List<Guid> ChildrenIds,
    bool IsDraft,
    DateTime StarDate,
    DateTime EndDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public class ParentBox(string name, string description, DateTime startDate, DateTime endDate)
    : Box(name, description)
{
    public List<Guid> ChildrenIds = [];
    public bool IsDraft;
    public DateTime StartDate = startDate;
    public DateTime EndDate = endDate;


    public void ToggleDraft()
    {
        IsDraft = !IsDraft;
    }

    public void AddChild(ChildBox child)
    {
        if (IsDraft)
        {
            ToggleDraft();
        }


        ChildrenIds.Add(child.Id);
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
        return new ParentData(Id, Name, Description, Archived, ChildrenIds, IsDraft,
            StartDate, EndDate, CreatedAt, UpdatedAt);
    }
}
