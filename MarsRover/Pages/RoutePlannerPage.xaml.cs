using MarsRover.ViewModels;

namespace MarsRover.Pages;

public partial class RoutePlannerPage : ContentPage
{
	public RoutePlannerPage(RoutePlannerPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}