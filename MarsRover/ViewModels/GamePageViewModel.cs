using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Services;

namespace MarsRover.ViewModels;

public partial class GamePageViewModel : ObservableObject
{
    private readonly MarsRoverService service;
    
    [ObservableProperty]
    private string token;

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

    public float BatteryLevel => battery / 100.0f;

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
        Token = await SecureStorage.GetAsync("token");
        Orientation = await SecureStorage.GetAsync("orientation");
        Name= await SecureStorage.GetAsync("name");
        Target = await SecureStorage.GetAsync("target");
        Position = await SecureStorage.GetAsync("position");
        Battery = 0;
    }

    [RelayCommand]
    public async Task MoveDirection(string direction)
    {
        var movement = await service.Move(token, direction);

        if (movement.message != "Too Many Requests")
        {
            Orientation = movement.orientation;
            await SecureStorage.SetAsync("orientation", Orientation);
            Battery = movement.batteryLevel;

            var col = movement.column;
            var row = movement.row;

            Position = $"{col}, {row}";
            await SecureStorage.SetAsync("position", Position);
        }
    }

    [RelayCommand]
    public async Task LeaveGame()
    {
        SecureStorage.RemoveAll();
        await Shell.Current.GoToAsync($"..");
    }
}
 