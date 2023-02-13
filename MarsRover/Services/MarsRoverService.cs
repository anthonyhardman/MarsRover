using MarsRover.Models;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace MarsRover.Services;

public class MarsRoverService : INotifyPropertyChanged
{
    private readonly HttpClient http;
    private readonly string gameDataPath;
    private GameData gameData;

    public event PropertyChangedEventHandler PropertyChanged;

    public GameData GameData
    {
        get => gameData;
        set
        {
            gameData = value;
            OnPropertyChanged(nameof(GameData));
        }
    }

    public MarsRoverService(HttpClient http)
    {
        this.http = http;
        var cacheDir = FileSystem.Current.CacheDirectory;
        gameDataPath = Path.Combine(cacheDir, "GameData.json");
        LoadGameData();
    }

    private void GameData_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(GameData));
    }

    public void LoadGameData()
    {
        if (File.Exists(gameDataPath))
        {
            var json = File.ReadAllText(gameDataPath);
            GameData = JsonSerializer.Deserialize<GameData>(json);
        }
        else
        {
            GameData = new GameData();
        }
    }

    public async Task LoadGameDataAsync()
    {
        if (File.Exists(gameDataPath))
        {
            using FileStream file = File.OpenRead(gameDataPath);
            GameData = await JsonSerializer.DeserializeAsync<GameData>(file);
        }
        else
        {
            GameData = new GameData();
        }
    }

    public async Task SaveGameDataAsync()
    {
        var json = JsonSerializer.Serialize(GameData);
        await File.WriteAllTextAsync(gameDataPath, json);
    }

    public async Task<bool> JoinGameAsync(string gameId, string name)
    {
        try
        {
            var response = await http.GetFromJsonAsync<JoinGameResponse>($"game/join?gameId={gameId}&name={name}");

            GameData.Token = response.Token;
            GameData.Name = name;
            GameData.Orientation = response.Orientation;
            GameData.Target = new Coordinate(response.TargetY, response.TargetX);
            GameData.PerseverancePosition = new Coordinate(response.StartingY, response.StartingX);
            GameData.IngenuityPosition = new Coordinate(response.StartingY, response.StartingX);
            GameData.HighResolutionMap = new();
            GameData.LowResolutionMap = response.LowResolutionMap;
            OnPropertyChanged(nameof(GameData));

            foreach (var cell in response.Neighbors)
            {
                GameData.HighResolutionMap[cell.Hash] = cell;
            }

            await SaveGameDataAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> MovePerseveranceAsync(string direction)
    {
        try
        {
            var response = await http.GetFromJsonAsync<MoveResponse>($"game/moveperseverance?token={GameData.Token}&direction={direction}");
            GameData.Orientation = response.Orientation;
            GameData.PerseveranceBattery = response.BatteryLevel;
            GameData.PerseverancePosition.X = response.Y;
            GameData.PerseverancePosition.Y = response.X;
            foreach (var cell in response.Neighbors)
            {
                GameData.HighResolutionMap[cell.Hash] = cell;
            }
            OnPropertyChanged(nameof(GameData));

            await SaveGameDataAsync();
            return response.Message;
        }
        catch
        {
            return "Too Many Requests";
        }
    }

    public async Task<string> MoveIngenuityAsync(int row, int col)
    {
        try
        {
            var response = await http.GetFromJsonAsync<MoveResponse>($"Game/MoveIngenuity?token={GameData.Token}&destinationRow={row}&destinationColumn={col}");
            gameData.IngenuityBattery = response.BatteryLevel;
            GameData.IngenuityPosition.X = response.Y;
            GameData.IngenuityPosition.Y = response.X;
            foreach (var cell in response.Neighbors)
            {
                GameData.HighResolutionMap[cell.Hash] = cell;
            }
            OnPropertyChanged(nameof(GameData));

            await SaveGameDataAsync();
            return response.Message;
        }
        catch
        {
            return "Too Many Requests";
        }
    }

    public async Task RunIngenuityRouteAsync(LinkedList<Coordinate> coordinates)
    {
        var x = gameData.IngenuityPosition;
        for (var curr = coordinates.First; curr != null; curr = curr.Next)
        {
            string message;
            do
            {
                message = await MoveIngenuityAsync((int)curr.Value.Y, (int)curr.Value.X);
            } while (message == "Too Many Requests");
        }
    }

    public async Task<string> GameStatusAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<GameStatusResponse>($"game/status?token={GameData.Token}");
            return response.status;
        }
        catch
        {
            return "Invalid";
        }
    }

    public async Task LeaveGameAsync()
    {
        GameData.Token = null;
        File.Delete(gameDataPath);
    }

    protected void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
            handler(this, e);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
}
