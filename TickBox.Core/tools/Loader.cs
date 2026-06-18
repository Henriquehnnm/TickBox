using System.Runtime.InteropServices;
using System.Text.Json;
using TickBox.Core.Models;

namespace TickBox.Core.tools;

public class Loader
{
    private ParentData _parent;
    private List<ChildData> _children = [];
    private List<ActionData> _actions = [];
    private string _globalPath;

    private Loader()
    {
        string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _globalPath = Path.Combine(local, "tickbox", "storage");
    }

    public static async Task<Loader> LoadParentAsync(Guid id)
    {
        var loader = new Loader();
        var path = Path.Combine(loader._globalPath, id.ToString(), "meta.json");
        var jsonString = await File.ReadAllTextAsync(path);
        loader._parent =
            JsonSerializer.Deserialize<ParentData>(jsonString) ??
            throw new InvalidOperationException(); // TODO - tratar null de forma mais elegante 1/3

        return loader;
    }

    public async Task LoadChildAsync()
    {
        var tasks =
            _parent.ChildrenIds.Select(async id =>
            {
                var path = Path.Combine(_globalPath, _parent.Id.ToString(), id.ToString(), "meta.json");
                var jsonString = await File.ReadAllTextAsync(path);
                return JsonSerializer.Deserialize<ChildData>(jsonString);
            });

        var results = await Task.WhenAll(tasks);
        foreach (var child in results)
        {
            _children.Add(child ??
                         throw new InvalidOperationException()); // TODO - Resolver null de forma mais elegante aqui tbm 2/3
        }
    }

    public async Task LoadActionAsync()
    {
        var tasks = _children.Select(async child =>
        {
            var actionTasks = child.ActionsIds.Select(async id =>
            {
                var path = Path.Combine(_globalPath, _parent.Id.ToString(), child.Id.ToString(), id.ToString(), "meta.json");
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
                _actions.Add(i ??
                            throw new InvalidOperationException()); // TODO -  Tratar  Null de forma mais elegante aqui tbm 3/3
            }
        }
    }
}
