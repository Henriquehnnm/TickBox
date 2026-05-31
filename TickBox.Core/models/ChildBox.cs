using NodaTime;
using System.Text.Json;
using System.Xml.Linq;

namespace TickBox.Core.Models;

public record ChildData(
    Guid Id,
    string Name,
    string Description,
    bool Archived,
    Guid ParentId,
    List<string> ActionsName,
    bool IsAbstract,
    Duration DurationTime,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class ChildBox : Box
{
    public Guid ParentId;
    public List<string> ActionsName = [];
    public bool IsAbstract = true;
    public Duration DurationTime;

    public ChildBox(string name, string description, ParentBox parent, Duration duration) : base(name, description)
    {
        ParentId = parent.Id;
        DurationTime = duration;
    }

    public void ToggleAbstract()
    {
        IsAbstract = !IsAbstract;
    }

    public void AddAction(ActionBox action)
    {
        if (IsAbstract)
        {
            ToggleAbstract();
        }

        ActionsName.Add(action.Name);
    }

    public async Task CreateAsync(string path, string filePath)
    {
        var option = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        if (!Directory.Exists(path))
        {
            await Task.Run(() => Directory.CreateDirectory(path));
        }


        string realPath = Path.Combine(path, filePath);

        var meta = Export();
        string json = JsonSerializer.Serialize(meta, option);
        await File.WriteAllTextAsync(realPath, json);
    }

    public ChildData Export()
    {
        return new ChildData(Id, Name, Description, Archived, ParentId, ActionsName,
            IsAbstract,
            DurationTime, CreatedAt, UpdatedAt);
    }
}
