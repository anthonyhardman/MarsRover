using MarsRover.Pages;

namespace MarsRover;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
		Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));	
	}
}
