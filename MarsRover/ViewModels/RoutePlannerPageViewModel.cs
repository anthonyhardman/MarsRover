using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Models;
using MarsRover.Services;


namespace MarsRover.ViewModels;

public partial class RoutePlannerPageViewModel : MapPageViewModel
{
	[ObservableProperty, NotifyPropertyChangedFor(nameof(ViableTargets))]
	private LinkedList<Coordinate> route;

	public IEnumerable<Coordinate> ViableTargets
	{
		get
		{
			var last = Route.Last.Value;
			var xS = Enumerable.Range((int)last.X - 2, 5);
			var yS = Enumerable.Range((int)last.Y - 2, 5);
			return xS
				.SelectMany(x => yS.Select(y => new Coordinate(x, y))
				.Where(c => !(c.X == last.X && c.Y == last.Y)));
		}
	}

	public RoutePlannerPageViewModel(MarsRoverService service) : base(service)
	{
		route = new();
		Route.AddLast(new Coordinate(GameData.IngenuityPosition.X, GameData.IngenuityPosition.Y));
    }

	[RelayCommand]
	public void Loaded()
	{
		Zoom = 75;
        originalOffset = new Coordinate(GameData.IngenuityPosition.X, GameData.IngenuityPosition.Y);
        PositionOffset = new Coordinate(GameData.IngenuityPosition.X, GameData.IngenuityPosition.Y);
    }

	[RelayCommand]
	public async Task RunRoute()
	{
		await service.RunIngenuityRouteAsync(Route);
		Route.Clear();
        Route.AddLast(new Coordinate(GameData.IngenuityPosition.X, GameData.IngenuityPosition.Y));
        InvalidateMap();
	}

    public void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var element = sender as View;
		var tapPostiion = e.GetPosition(element);

		var origin = new Coordinate((float)element.Height / 2.0f, (float)element.Width / 2.0f);

		var col = (float)Math.Round((tapPostiion.Value.X - origin.Y) / Zoom + PositionOffset.Y);
        var row = (float)Math.Round((tapPostiion.Value.Y - origin.X) / -Zoom + PositionOffset.X);

		Route.AddLast(new Coordinate(row, col));
		OnPropertyChanged(nameof(Route));
		OnPropertyChanged(nameof(ViableTargets));
    }
}
