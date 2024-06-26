﻿using DietAppClient.Views;

namespace DietAppClient;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(UserDataPage), typeof(UserDataPage));
        Routing.RegisterRoute(nameof(EatingsPage), typeof(EatingsPage));
        Routing.RegisterRoute(nameof(MenageEatingPage), typeof(MenageEatingPage));
        Routing.RegisterRoute(nameof(RecordsPage), typeof(RecordsPage));
        Routing.RegisterRoute(nameof(MenageRecordPage), typeof(MenageRecordPage));
        Routing.RegisterRoute(nameof(StattPage), typeof(StattPage));
    }
}
