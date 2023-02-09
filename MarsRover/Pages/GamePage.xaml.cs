using MarsRover.ViewModels;

namespace MarsRover.Pages;

public partial class GamePage : ContentPage
{
	public GamePage(GamePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.InvalidateMap = GraphicsView.Invalidate;
        MapView.BindingContext = vm;
    }
}