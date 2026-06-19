using NodaTime;
using System.Text.Json;

namespace TickBox.Core.Models;

public record ChildData(
    Guid Id,
    string Name,
    string Description,
    bool Archived,
    Guid ParentId,
    List<Guid> ActionsIds,
    bool IsAbstract,
    Duration CachedDuration,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class ChildBox(string name, string description, ParentBox parent)
    : Box(name, description)
{
    public Guid ParentId = parent.Id;
    public List<Guid> ActionsIds = [];
    public bool IsAbstract = true;
    private Duration _cachedDuration = Duration.Zero; // Melhor assim do que null.

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

        ActionsIds.Add(action.Id);
        _cachedDuration += action.DurationTime;
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
        return new ChildData(Id, Name, Description, Archived, ParentId, ActionsIds,
            IsAbstract,
            _cachedDuration, CreatedAt, UpdatedAt);
    }
}
