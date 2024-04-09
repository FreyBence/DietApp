using CommunityToolkit.Maui;
using DietAppClient.Data;
using DietAppClient.Logics;
using DietAppClient.ViewModels;
using DietAppClient.Views;

namespace DietAppClient;

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

        builder.Services.AddSingleton<IEatingRepository, EatingRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IRecordRepository, RecordRepository>();
        builder.Services.AddSingleton<IStattLogic, StattLogic>();
        builder.Services.AddSingleton<IBaselineLogic, BaselineLogic>();
        builder.Services.AddSingleton<IDailyParamsLogic, DailyParamsLogic>();
        builder.Services.AddSingleton<IInterventionLogic, InterventionLogic>();
        builder.Services.AddSingleton<IBodyModelLogic, BodyModelLogic>();

        builder.Services.AddSingleton<UserDataPage>();
        builder.Services.AddSingleton<MenageEatingPage>();
        builder.Services.AddSingleton<EatingsPage>();
        builder.Services.AddSingleton<RecordsPage>();
        builder.Services.AddSingleton<MenageRecordPage>();
        builder.Services.AddSingleton<StattPage>();

        builder.Services.AddSingleton<UserDataViewModel>();
        builder.Services.AddSingleton<EatingsViewModel>();
        builder.Services.AddSingleton<MenageEatingViewModel>();
        builder.Services.AddSingleton<StattViewModel>();
        builder.Services.AddSingleton<RecordsViewModel>();
        builder.Services.AddSingleton<MenageRecordViewModel>();

        return builder.Build();
    }
}
