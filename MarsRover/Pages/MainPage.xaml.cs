using MarsRover.ViewModels;

namespace MarsRover;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}

