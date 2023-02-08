using CommunityToolkit.Mvvm.ComponentModel;
using MarsRover.Models;
using MarsRover.Services;

namespace MarsRover.ViewModels;

public partial class MapPageViewModel : ObservableObject
{
    private readonly MarsRoverService service;

    [ObservableProperty]
    private Dictionary<long, Models.Cell> highResolutionMap;

    [ObservableProperty]
    private IEnumerable<LowResolutionMap> lowResolutionMap;

    [ObservableProperty]
    private float zoom;

    [ObservableProperty]
    private Coordinate positionOffset;

    [ObservableProperty]
    private Coordinate perseverancePosition;

    public delegate void InvalidateMapDelegate();

    public InvalidateMapDelegate InvalidateMap { get; set; }

    private Coordinate originalOffset;

    public MapPageViewModel(MarsRoverService service)
    {
        this.service = service;
        HighResolutionMap = service.GameData.HighResolutionMap;
        lowResolutionMap = service.GameData.LowResolutionMap;
        Zoom = 25;
        perseverancePosition = service.GameData.Position;
        originalOffset = new Coordinate(perseverancePosition.X, perseverancePosition.Y);
        PositionOffset = new Coordinate(perseverancePosition.X, perseverancePosition.Y);
        this.PropertyChanged += MapPageViewModel_PropertyChanged; 
    }

    private void MapPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Zoom))
        {
            InvalidateMap();
        }
    }

    public void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            PositionOffset = new Coordinate(originalOffset.X + e.TotalY * (1/zoom),
                originalOffset.Y - e.TotalX * (1/zoom));
            InvalidateMap();
        }
        else if (e.StatusType == GestureStatus.Completed)
        {
            originalOffset = new Coordinate(PositionOffset.X, PositionOffset.Y);
        }
    }
}
