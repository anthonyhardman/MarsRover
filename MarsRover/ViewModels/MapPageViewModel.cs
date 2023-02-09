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

    [ObservableProperty]
    private string perseveranceOrientation;

    [ObservableProperty]
    private Coordinate ingenuityPosition;

    public delegate void InvalidateMapDelegate();

    public InvalidateMapDelegate InvalidateMap { get; set; }

    private Coordinate originalOffset;

    public MapPageViewModel(MarsRoverService service)
    {
        this.service = service;
        Zoom = 25;
        HighResolutionMap = service.GameData.HighResolutionMap;
        lowResolutionMap = service.GameData.LowResolutionMap;
        PerseverancePosition = service.GameData.PerseverancePosition;
        PerseveranceOrientation = service.GameData.Orientation;
        originalOffset = new Coordinate(PerseverancePosition.X, PerseverancePosition.Y);
        PositionOffset = new Coordinate(PerseverancePosition.X, PerseverancePosition.Y);
        ingenuityPosition = service.GameData.IngenuityPosition;
        
        this.PropertyChanged += MapPageViewModel_PropertyChanged;
        service.PropertyChanged += Service_PropertyChanged;
    }

    private void Service_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        HighResolutionMap = service.GameData.HighResolutionMap;
        LowResolutionMap = service.GameData.LowResolutionMap;
        PerseverancePosition = service.GameData.PerseverancePosition;
        IngenuityPosition = service.GameData.IngenuityPosition;
        PerseveranceOrientation = service.GameData.Orientation;
        if (PerseverancePosition != null)
        {
            originalOffset = new Coordinate(PerseverancePosition.X, PerseverancePosition.Y);
            PositionOffset = new Coordinate(PerseverancePosition.X, PerseverancePosition.Y);
        }
    }

    private void MapPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        InvalidateMap();
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
