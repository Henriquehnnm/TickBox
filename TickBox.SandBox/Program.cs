using NodaTime;
using TickBox.Core.tools;

var durt = Duration.FromDays(8);
var now = new DateTime();
var data = await TickBox.Core.TickBox.CreateParentAsync("Hello Parent", "", now, now);
await data.CreateChildAsync("Hello Child", "", durt);
await data.CreateChildAsync("Hello Child 2", "", durt);
await data.CreateActionAsync("Hello Action", "", durt, "# Hello World from TickBox!", data.Children[0].Id);
await data.CreateActionAsync("Hello Action 2", "", durt, "# Hello World from TickBox!", data.Children[1].Id);
var load = await Loader.LoadParentAsync(data.Parent.Id);
await load.LoadChildAsync();
await load.LoadActionAsync();