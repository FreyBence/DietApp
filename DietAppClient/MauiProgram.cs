using DietAppClient.Data;
using DietAppClient.Logics;
using DietAppClient.ViewModels;
using DietAppClient.Views;
using Microsoft.Extensions.Logging;

namespace DietAppClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<IEatingRepository, EatingRepository>();
		builder.Services.AddSingleton<IUserRepository, UserRepository>();
		builder.Services.AddSingleton<IStattLogic, StattLogic>();

		builder.Services.AddSingleton<UserDataPage>();
		builder.Services.AddSingleton<MenageEatingPage>();
		builder.Services.AddSingleton<EatingsPage>();
		builder.Services.AddSingleton<StattPage>();

		builder.Services.AddSingleton<UserDataViewModel>();
		builder.Services.AddSingleton<EatingsViewModel>();
        builder.Services.AddSingleton<MenageEatingViewModel>();
        builder.Services.AddSingleton<StattViewModel>();

        return builder.Build();
	}
}
