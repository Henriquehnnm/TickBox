using NodaTime;
using TickBox.Core.tools;

var hDurt  = Duration.FromHours(3);
var now = new DateTime();
var data = await TickBox.Core.TickBox.CreateParentAsync("Hello Parent", "", now, now);
await data.CreateChildAsync("Hello Child", "");
await data.CreateChildAsync("Hello Child 2", "");
var children = data.GetChildren();
await data.CreateActionAsync("Hello Action", "", hDurt, "# Hello World from TickBox!", children[0].Id);
await data.CreateActionAsync("Hello Action 2", "", hDurt, "# Hello World from TickBox!", children[1].Id);
var load = await Loader.LoadParentAsync(data.Parent.Id);
await load.LoadChildAsync();
await load.LoadActionAsync();