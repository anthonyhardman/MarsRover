using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Pages;
using MarsRover.Services;

namespace MarsRover.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly MarsRoverService service;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string gameId;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string name;

    public MainPageViewModel(MarsRoverService service)
    {
        this.service = service;
    }

    private bool canJoinGame => gameId != null && name != null;


    [RelayCommand]
    public async Task Loaded()
    {
        GameId = null;
        var token = await SecureStorage.GetAsync("token");
        var gameStatus = await service.GameStatus(token);
        if (token != null && gameStatus != "Invalid")
        {
            await Shell.Current.GoToAsync($"{nameof(GamePage)}");
        }
    }

    [RelayCommand(CanExecute = nameof(canJoinGame))]
    public async Task JoinGame()
    {
        var response = await service.JoinGame(GameId, Name);
        var token = response.token;
        var orientation = response.orientation;
        var targetColumn = response.targetColumn.ToString();
        var targetRow = response.targetRow.ToString();
        var startingColumn = response.startingColumn.ToString();
        var startingRow = response.startingRow.ToString();

        await SecureStorage.SetAsync("token", token);
        await SecureStorage.SetAsync("orientation", orientation);
        await SecureStorage.SetAsync("name", Name);
        await SecureStorage.SetAsync("target", $"{targetColumn}, {targetRow}");
        await SecureStorage.SetAsync("position", $"{startingColumn}, {startingRow}");

        await Shell.Current.GoToAsync($"{nameof(GamePage)}");
    }
}
