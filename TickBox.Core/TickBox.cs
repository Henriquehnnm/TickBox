using System.Text.Json;
using NodaTime;
using TickBox.Core.Models;

namespace TickBox.Core;

enum BoxType
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
    private ParentBox Parent;
    private List<ChildBox> Children = [];
    private List<ActionBox> Actions = [];
    private string globalPath;

    private TickBox()
    {
        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        globalPath = Path.Combine(local, "tickbox", "storage");
    }

    public static async Task<TickBox> CreateParentAsync(string name, string description, DateTime startDate,
        DateTime endDate)
    {
        var tickbox = new TickBox();
        tickbox.Parent = new ParentBox(name, description, startDate, endDate);
        await tickbox.StartStorageAsync();
        return tickbox;
    }

    private async Task StartStorageAsync()
    {
        try
        {
            string parentPath = Path.Combine(globalPath, Parent.Name);
            string metaPath = Path.Combine(parentPath, "meta.json");

            await Parent.CreateAsync(parentPath, metaPath);
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
        }
    }

    private async Task refreshData(string filePath, BoxType type, string childName = "")
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
            var real = GetChildByName(childName);
            if (real is not null)
            {
                string meta = JsonSerializer.Serialize(real.Export(), option);
                await File.WriteAllTextAsync(filePath, meta);
            }
        }
    }

    private ChildBox? GetChildByName(string name)
    {
        return Children.Find(c => c.Name == name);
    }

    public async Task CreateChildAsync(string name, string description, Duration durarionTime)
    {
        var child = new ChildBox(name, description, Parent, durarionTime);
        Children.Add(child);
        Parent.AddChild(child);
        string folderPath = Path.Combine(globalPath, Parent.Name, child.Name);
        await child.CreateAsync(folderPath, "meta.json");
        await refreshData(Path.Join(globalPath, Parent.Name, "meta.json"),
            BoxType.ParentBox);
    }

    public async Task CreateActionAsync(string name, string description, Duration durationTime, string content,
        string childName)
    {
        var real = GetChildByName(childName);

        if (real is not null)
        {
            var action = new ActionBox(name, description, real, content, durationTime);
            Actions.Add(action);
            real.AddAction(action);
            string folderPath = Path.Combine(globalPath, Parent.Name, real.Name, action.Name);
            await action.CreateAsync(folderPath, "meta.json");
            string childMetaPath =
                Path.Combine(globalPath, Parent.Name, real.Name,
                    "meta.json");
            string contentPath = Path.Combine(folderPath, "content.md");
            await File.WriteAllTextAsync(contentPath, action.Content);
            await refreshData(childMetaPath, BoxType.ChildBox, real.Name);
        }
    }
}
