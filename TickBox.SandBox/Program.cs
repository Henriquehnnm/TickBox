using System.Data;
using TickBox.Core;
using NodaTime;

var durt = Duration.FromDays(8);
var now = new DateTime();
var data = await TickBox.Core.TickBox.CreateParentAsync("Hello Parent", "", now, now);
data.CreateChildAsync("Hello Child", "", durt);
data.CreateActionAsync("Hello Action", "", durt, "# Hello World from TickBox!", "Hello Child");
