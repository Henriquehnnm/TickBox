using System.Text.Json;
using NodaTime;
using TickBox.Core.Models;

namespace TickBox.Core;

internal enum BoxType
{
    ParentBox,
    ChildBox,
    ActionBox
}

/*
 * TickBox Main Class
 * Code Level: GAMBIARRA SUPREMA
 */
public class TickBox
{
    public ParentBox Parent { get; private set; }
    private readonly List<ChildBox> _children = [];
    private readonly List<ActionBox> _actions = [];
    private readonly string _globalPath;

    private TickBox()
    {
        var local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _globalPath = Path.Combine(local, "tickbox", "storage");
    }

    public static async Task<TickBox> CreateParentAsync(string name, string description, DateTime startDate,
        DateTime endDate)
    {
        var tickbox = new TickBox();
        tickbox.Parent = new ParentBox(name, description, startDate, endDate);
        await tickbox.StartStorageAsync();
        return tickbox;
    }

    public IReadOnlyList<ChildBox> GetChildren()
    {
        return _children.AsReadOnly();
    }

    public IReadOnlyList<ActionBox> GetActions()
    {
        return _actions.AsReadOnly();
    }

    private async Task StartStorageAsync()
    {
        try
        {
            string parentPath = Path.Combine(_globalPath, Parent.Id.ToString());
            string metaPath = Path.Combine(parentPath, "meta.json");

            await Parent.CreateAsync(parentPath, metaPath);
        }
        catch (Exception err)
        {
            throw new InvalidOperationException($"Storage initialization error: {err}");
        }
    }

    private async Task RefreshData(string filePath, BoxType type, Guid childId = default)
    {
        var option = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        if (type == BoxType.ParentBox)
        {
            string meta = JsonSerializer.Serialize(Parent.Export(), option);
            await File.WriteAllTextAsync(filePath, meta);
        }
        else if (type == BoxType.ChildBox)
        {
            var real = GetChildById(childId);
            if (real is not null)
            {
                string meta = JsonSerializer.Serialize(real.Export(), option);
                await File.WriteAllTextAsync(filePath, meta);
            }
        }
    }

    private ChildBox? GetChildById(Guid id)
    {
        return _children.Find(c => c.Id == id);
    }

    public async Task CreateChildAsync(string name, string description)
    {
        var child = new ChildBox(name, description, Parent);
        _children.Add(child);
        Parent.AddChild(child);
        string folderPath = Path.Combine(_globalPath, Parent.Id.ToString(), child.Id.ToString());
        await child.CreateAsync(folderPath, "meta.json");
        await RefreshData(Path.Combine(_globalPath, Parent.Id.ToString(), "meta.json"),
            BoxType.ParentBox);
    }

    public async Task CreateActionAsync(string name, string description, Duration durationTime, string content,
        Guid childId)
    {
        var real = GetChildById(childId);

        if (real is not null)
        {
            var action = new ActionBox(name, description, real, content, durationTime);
            _actions.Add(action);
            real.AddAction(action);
            string folderPath =
                Path.Combine(_globalPath, Parent.Id.ToString(), real.Id.ToString(), action.Id.ToString());
            await action.CreateAsync(folderPath, "meta.json");
            string childMetaPath =
                Path.Combine(_globalPath, Parent.Id.ToString(), real.Id.ToString(),
                    "meta.json");
            string contentPath = Path.Combine(folderPath, "content.md");
            await File.WriteAllTextAsync(contentPath, action.Content);
            await RefreshData(childMetaPath, BoxType.ChildBox, real.Id);
        }
    }
}