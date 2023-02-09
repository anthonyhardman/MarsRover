using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Pages;
using MarsRover.Services;
using System.Collections.ObjectModel;

namespace MarsRover.ViewModels;

public partial class GamePageViewModel : ObservableObject
{
    private readonly MarsRoverService service;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(OrientationSymbol))]
    private string orientation;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(BatteryLevel))]
    private int battery;

    [ObservableProperty]
    private string name;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(PositionAndTarget))]
    private string target;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(PositionAndTarget))]
    private string position;

    public float BatteryLevel => Battery / 18000.0f;

    public string PositionAndTarget => $"⌖{Position}    ⚑{Target}";

    public string OrientationSymbol
    {
        get
        {
            switch (Orientation)
            {
                case "North":
                    return "⇈";
                case "East":
                    return "⇉";
                case "South":
                    return "⇊";
                case "West":
                    return "⇇";
                default:
                    return "You're Drunk";
            }
        }
    }

    public GamePageViewModel(MarsRoverService service)
    {
        this.service = service;
    }

    [RelayCommand]
    public async Task Loaded()
    {
        Orientation = service.GameData.Orientation;
        Name = service.GameData.Name;
        Target = service.GameData.Target.ToString();
        Position = service.GameData.PerseverancePosition.ToString();    
        Battery = 0;
    }

    [RelayCommand]
    public async Task MoveDirection(string direction)
    {
        var message = await service.MovePerseveranceAsync(direction);

        if (message != "Too Many Requests")
        {
            Orientation = service.GameData.Orientation;
            Battery = service.GameData.PerseveranceBattery;
            Position = service.GameData.PerseverancePosition.ToString();
        }
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
}
 