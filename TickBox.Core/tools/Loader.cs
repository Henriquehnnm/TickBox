using System.Text.Json;
using TickBox.Core.Models;

namespace TickBox.Tools;

public class Loader
{
    private ParentData parent;
    private List<ChildData> children = [];
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
        loader.parent =
            JsonSerializer.Deserialize<ParentData>(jsonString) ??
            throw new InvalidOperationException(); // TODO - tratar null de forma mais elegante 1/3

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
            children.Add(child ??
                         throw new InvalidOperationException()); // TODO - Resolver null de forma mais elegante aqui tbm 2/3
        }
    }

    public async Task LoadActionAsync()
    {
        var tasks = children.Select(async child =>
        {
            var actionTasks = child.ActionsName.Select(async name =>
            {
                var path = Path.Combine(globalPath, parent.Name, child.Name, name, "meta.json");
                var jsonString = await File.ReadAllTextAsync(path);
                return JsonSerializer.Deserialize<ActionData>(jsonString);
            });
            return await Task.WhenAll(actionTasks);
        });

        var results = await Task.WhenAll(tasks);
        foreach (var a in
                 results) // Pelo robozin do .NET, nao me julguem por esses 2 loops, eu tenho trauma de if e for...
        {
            foreach (var i in a)
            {
                actions.Add(i ??
                            throw new InvalidOperationException()); // TODO -  Tratar  Null de forma mais elegante aqui tbm 3/3
            }
        }
    }
}
