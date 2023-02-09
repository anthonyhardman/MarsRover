using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Helpers;
using MarsRover.Models;
using MarsRover.Pages;
using MarsRover.Services;
using System.Collections.ObjectModel;

namespace MarsRover.ViewModels;

public partial class GamePageViewModel : ObservableObject
{
    private readonly MarsRoverService service;

    [ObservableProperty]
    private Coordinate positionOffset;


    [ObservableProperty, 
        NotifyPropertyChangedFor(nameof(PerseveranceBatteryGuage), 
                                 nameof(IngenuityeBatteryGuage),
                                 nameof(PerseverancePositionDisplay),
                                 nameof(IngenuityPositionDisplay),
                                 nameof(TargetDisplay))]
    private GameData gameData;

    public delegate void InvalidateMapDelegate();

    public InvalidateMapDelegate InvalidateMap { get; set; }

    public float PerseveranceBatteryGuage => GameData.PerseveranceBattery / 18000.0f * 100.0f;
    public float IngenuityeBatteryGuage => GameData.IngenuityBattery / 18000.0f * 100.0f;
    public string PerseverancePositionDisplay => $"{MaterialDesignIconFonts.MapMarker} {GameData.PerseverancePosition.Y}, {GameData.PerseverancePosition.X}";
    public string IngenuityPositionDisplay => $"{MaterialDesignIconFonts.MapMarker} {GameData.IngenuityPosition.X}, {GameData.IngenuityPosition.Y}";
    public string TargetDisplay => $"{MaterialDesignIconFonts.Bullseye} {GameData.Target.Y}, {GameData.Target.X}";

    public GamePageViewModel(MarsRoverService service)
    {
        this.service = service;
        this.service.PropertyChanged += Service_PropertyChanged;
        this.PropertyChanged += GamePageViewModel_PropertyChanged;
        GameData = service.GameData;
        PositionOffset = new Coordinate(GameData.PerseverancePosition.X, GameData.PerseverancePosition.Y);
       
    }

    [RelayCommand]
    public async Task Loaded()
    {
        GameData = service.GameData;
    }

    [RelayCommand]
    public async Task MoveDirection(string direction)
    {
        var message = await service.MovePerseveranceAsync(direction);
    }

    [RelayCommand]
    public async Task LeaveGame()
    {
        await service.LeaveGameAsync();
        await Shell.Current.GoToAsync($"..");
    }

    [RelayCommand]
    public async Task NaviateToMapPage()
    {
        await Shell.Current.GoToAsync($"{nameof(MapPage)}");
    }

    private void Service_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        GameData = service.GameData;
        OnPropertyChanged(nameof(PerseverancePositionDisplay));
        OnPropertyChanged(nameof(PerseveranceBatteryGuage));
        OnPropertyChanged(nameof(IngenuityPositionDisplay));
        OnPropertyChanged(nameof(IngenuityeBatteryGuage));


        if (GameData.PerseverancePosition != null)
        {
            PositionOffset = new Coordinate(GameData.PerseverancePosition.X, GameData.PerseverancePosition.Y);
        }
    }

    private void GamePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (InvalidateMap != null)
        {
            InvalidateMap();
        }
    }
}
 