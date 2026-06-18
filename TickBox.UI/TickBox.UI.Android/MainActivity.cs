using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace TickBox.UI.Android;

[Activity(
    Label = "TickBox.UI.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
}
