using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Models;
using MarsRover.Services;

namespace MarsRover.ViewModels;

public partial class MapPageViewModel : ObservableObject
{
    protected readonly MarsRoverService service;

    [ObservableProperty]
    private float zoom;

    [ObservableProperty]
    private Coordinate positionOffset;

    [ObservableProperty]
    private GameData gameData;

    public delegate void InvalidateMapDelegate();
    public InvalidateMapDelegate InvalidateMap { get; set; }

    protected Coordinate originalOffset;

    protected bool panningInvalid = false;

    protected DateTime lastCompletedPinch;

    public MapPageViewModel(MarsRoverService service)
    {
        this.service = service;
        Zoom = 25;
        GameData = service.GameData;
        originalOffset = new Coordinate(GameData.PerseverancePosition.X, GameData.PerseverancePosition.Y);
        PositionOffset = new Coordinate(GameData.PerseverancePosition.X, GameData.PerseverancePosition.Y);

        this.PropertyChanged += MapPageViewModel_PropertyChanged;
        service.PropertyChanged += Service_PropertyChanged;
    }

    protected void Service_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        GameData = service.GameData;
        InvalidateMap();
    }

    protected void MapPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (InvalidateMap != null)
        {
            InvalidateMap();
        }
    }

    public void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (DateTime.Now > lastCompletedPinch.AddMilliseconds(250) && !panningInvalid)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                PositionOffset = new Coordinate((float)(originalOffset.X + e.TotalY * (1 / Zoom)),
                    (float)(originalOffset.Y - e.TotalX * (1 / Zoom)));
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                originalOffset = new Coordinate(PositionOffset.X, PositionOffset.Y);
            }
        }
        else if (panningInvalid && e.StatusType == GestureStatus.Completed)
        {
            panningInvalid = false;
        }
        else
        {
            panningInvalid = true;
        }
    }

    public void PinchGestrueRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Running)
        {
            Zoom *= (float)e.Scale;
        }
        else if (e.Status == GestureStatus.Completed)
        {
            lastCompletedPinch = DateTime.Now;
        }
    }

    [RelayCommand]
    public void GoToPerseverance()
    {
        originalOffset = new Coordinate(GameData.PerseverancePosition.X, GameData.PerseverancePosition.Y);
        PositionOffset = new Coordinate(GameData.PerseverancePosition.X, GameData.PerseverancePosition.Y);
    }

    [RelayCommand]
    public void GoToIngenuity()
    {
        originalOffset = new Coordinate(GameData.IngenuityPosition.X, GameData.IngenuityPosition.Y);
        PositionOffset = new Coordinate(GameData.IngenuityPosition.X, GameData.IngenuityPosition.Y);
    }

    [RelayCommand]
    public void GoToTarget()
    {
        originalOffset = new Coordinate(GameData.Target.X, GameData.Target.Y);
        PositionOffset = new Coordinate(GameData.Target.X, GameData.Target.Y);
    }
}
