using System.Text.Json;
using TickBox.Core.Models;

namespace TickBox.Tools;

public class Loader
{
    private ParentData parent;
    private List<ChildData> childs = [];
    private List<ActionData> actions = [];
    private string globalPath;

    private Loader()
    {
        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        globalPath = Path.Combine(local, "tickbox", "storage");
    }

    public static async Task<Loader> LoadParentAsync(string name)
    {
        var loader = new Loader();
        var path = Path.Combine(loader.globalPath, name, "meta.json");
        var jsonString = await File.ReadAllTextAsync(path);
        loader.parent = JsonSerializer.Deserialize<ParentData>(jsonString); // TODO - tratar null 1/2
        return loader;
    }

    public async Task LoadChildAsync()
    {
        var tasks =
            parent.ChildrenName.Select(async name =>
            {
                var path = Path.Combine(globalPath, parent.Name, name, "meta.json");
                var jsonString = await File.ReadAllTextAsync(path);
                return JsonSerializer.Deserialize<ChildData>(jsonString);
            });

        var results = await Task.WhenAll(tasks);
        foreach (var child in results)
        {
            childs.Add(child); // TODO - Resolver null aqui tbm 2/2
        }
    }
}
