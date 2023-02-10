using CommunityToolkit.Maui;
using MarsRover.Components;
using MarsRover.Pages;
using MarsRover.Services;
using MarsRover.ViewModels;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace MarsRover;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
			});
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhkVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jS35adkVmX3xccHZSQQ==;Mgo+DSMBPh8sVXJ0S0J+XE9AflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TdEVqWX9bdndXRmRbVg==;ORg4AjUWIQA/Gnt2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdjUH9edXJWRGBaVUM=;MTA5NDYwOUAzMjMwMmUzNDJlMzBNTkthSzl0NEZEUzhYeEJhanBHYm1ibWllREFUbU40Q0s1L3ZhODN1V0JVPQ==;MTA5NDYxMEAzMjMwMmUzNDJlMzBQWG1SSmplSEFEQ28xQUhpcFhrcjNWT0V4YWhyc2lPcmpZQXF0UEt3L3U0PQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFtKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdUVhWXZecHFTRWJdVEJ+;MTA5NDYxMkAzMjMwMmUzNDJlMzBkb1NqSEtPY0k1UHNCbkdvT1U3UXV3Mzd0akh5UW92S0Q3Vi9LNFhucWRVPQ==;MTA5NDYxM0AzMjMwMmUzNDJlMzBFTWVnN1BkMmJMM2U3RlY5SjM0OGxXeFZkQlpaMWVvaFF5cUJqMmlTMTFjPQ==;Mgo+DSMBMAY9C3t2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdjUH9edXJWRGNeV0Y=;MTA5NDYxNUAzMjMwMmUzNDJlMzBhRmFXWXhRU21PSmgybTZTa2JmRzlNbG05WFRUMEl5UzhQQzlaaGtDeEl3PQ==;MTA5NDYxNkAzMjMwMmUzNDJlMzBneEVlM0ROblIvaDdsMHVXc0pTQVRQWXlvbklwTUl6RFRNMWtmZXZkRWg0PQ==;MTA5NDYxN0AzMjMwMmUzNDJlMzBkb1NqSEtPY0k1UHNCbkdvT1U3UXV3Mzd0akh5UW92S0Q3Vi9LNFhucWRVPQ==");
#if DEBUG
        builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();

		builder.Services.AddSingleton<GamePage>();
		builder.Services.AddSingleton<GamePageViewModel>();

		builder.Services.AddSingleton<MapPage>();
		builder.Services.AddSingleton<MapPageViewModel>();

		builder.Services.AddSingleton<RoutePlannerPage>();
		builder.Services.AddSingleton<RoutePlannerPageViewModel>();	

		builder.Services.AddSingleton<MarsRoverService>();
		builder.Services.AddSingleton<AlertService>();	
		
		builder.Services.AddSingleton(sp => new HttpClient
		{
			BaseAddress = new Uri("https://snow-rover.azurewebsites.net/")
        });

		return builder.Build();
	}
}
