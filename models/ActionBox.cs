using System.Text.Json;

namespace TickBox.Core.Models;

using NodaTime;

public record ActionData(
    Guid Id,
    string Name,
    string Description,
    bool Archived,
    Guid ChildId,
    Duration DurationTime,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class ActionBox : Box
{
    public Guid ChildId;
    public string Content;
    public Duration DurationTime;

    public ActionBox(string name, string description, ChildBox child, string content, Duration durationTime) : base(
        name, description)
    {
        this.ChildId = child.Id;
        this.Content = content;
        this.DurationTime = durationTime;
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

    public ActionData Export()
    {
        return new ActionData(this.Id, this.Name, this.Description, this.Archived, this.ChildId,
            this.DurationTime,
            this.CreatedAt, this.UpdatedAt);
    }
}
