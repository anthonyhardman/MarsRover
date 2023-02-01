using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsRover.Services;

namespace MarsRover.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly MarsRoverService service;
    
    [ObservableProperty]
    private string gameId;

    [ObservableProperty]
    private string name;

    public MainPageViewModel(MarsRoverService service)
    {
        this.service = service;
    }

    [RelayCommand]
    public async Task JoinGame()
    {
        var response = await service.JoinGame(GameId, Name);
        var token = response.token;
    }
}
