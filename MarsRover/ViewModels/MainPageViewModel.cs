using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Models;
using MarsRover.Pages;
using MarsRover.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarsRover.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly MarsRoverService service;
    private readonly AlertService alert;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string gameId;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string name;

    public MainPageViewModel(MarsRoverService service, AlertService alert)
    {
        this.service = service;
        this.alert = alert;
    }

    private bool canJoinGame => GameId != null && Name != null;

    [RelayCommand]
    public async Task Loaded()
    {
        var gameStatus = await service.GameStatusAsync();
        if (gameStatus != "Invalid")
        {
            await Shell.Current.GoToAsync($"{nameof(GamePage)}");
        }
    }

    [RelayCommand(CanExecute = nameof(canJoinGame))]
    public async Task JoinGame()
    {
        var joined = await service.JoinGameAsync(GameId, Name);

        if (joined)
        {
            await Shell.Current.GoToAsync($"{nameof(GamePage)}");
        }
        else
        {
            await alert.ShowAlertAsync("Invalid Game ID", "");
        }
    }
}
