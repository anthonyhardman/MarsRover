using MarsRover.ViewModels;

namespace MarsRover.Pages;

public partial class MapPage : ContentPage
{
	public MapPage(MapPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		MapView.BindingContext = vm;
		vm.InvalidateMap = GraphicsView.Invalidate;
		PanGestureRecognizer.PanUpdated += vm.PanGestureRecognizer_PanUpdated;
        PinchGestrueRecognizer.PinchUpdated += vm.PinchGestrueRecognizer_PinchUpdated;
	}
}