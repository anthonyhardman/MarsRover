using MarsRover.ViewModels;

namespace MarsRover.Pages;

public partial class RoutePlannerPage : ContentPage
{
	public RoutePlannerPage(RoutePlannerPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		RouteView.BindingContext = vm;
		vm.InvalidateMap = GraphicsView.Invalidate;
        PanGestureRecognizer.PanUpdated += vm.PanGestureRecognizer_PanUpdated;
        PinchGestrueRecognizer.PinchUpdated += vm.PinchGestrueRecognizer_PinchUpdated;
        TapGestureRecognizer.Tapped += vm.TapGestureRecognizer_Tapped;
    }
}