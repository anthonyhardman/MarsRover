using CommunityToolkit.Maui;
using MarsRover.Pages;
using MarsRover.Services;
using MarsRover.ViewModels;
using Microsoft.Extensions.Logging;

namespace MarsRover;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();

		builder.Services.AddSingleton<GamePage>();
		builder.Services.AddSingleton<GamePageViewModel>();

		builder.Services.AddSingleton<MarsRoverService>();

		builder.Services.AddSingleton(sp => new HttpClient
		{
			BaseAddress = new Uri("https://snow-rover.azurewebsites.net/")
        });


		return builder.Build();
	}
}
