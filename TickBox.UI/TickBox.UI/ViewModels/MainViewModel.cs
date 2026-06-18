using CommunityToolkit.Mvvm.ComponentModel;

namespace TickBox.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to ";
    
    [ObservableProperty]
    private int _num = 0;
    
    void sum()
    {
        _num++;
    }
}
