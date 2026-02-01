using SmashScheduler.Infrastructure.Persistence;

namespace SmashScheduler;

public partial class App : Application
{
    public App(DatabaseInitialiser databaseInitialiser)
    {
        InitializeComponent();
        MainPage = new AppShell();
        InitialiseDatabaseAsync(databaseInitialiser);
    }

    private async void InitialiseDatabaseAsync(DatabaseInitialiser databaseInitialiser)
    {
        await databaseInitialiser.InitialiseAsync();
    }
}
