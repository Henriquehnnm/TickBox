using System.Data;
using TickBox.Core;
using NodaTime;

var durt = Duration.FromDays(8);
var now = new DateTime();
var data = await TickBox.Core.TickBox.CreateParentAsync("Hello Parent", "", now, now);
await data.CreateChildAsync("Hello Child", "", durt);
await data.CreateChildAsync("Hello Child 2", "", durt);
await data.CreateActionAsync("Hello Action", "", durt, "# Hello World from TickBox!", "Hello Child");
await data.CreateActionAsync("Hello Action 2", "", durt, "# Hello World from TickBox!", "Hello Child 2");
var load = await TickBox.Tools.Loader.LoadParentAsync("Hello Parent");
await load.LoadChildAsync();
await load.LoadActionAsync();
