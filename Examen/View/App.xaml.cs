using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using View.Pages;
using ViewModel.PagesViewModel;
using ViewModel.Services.Classes;
using ViewModel.Services.Interfaces;

namespace View;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; }

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<INavigateService, NavigateService>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddTransient<LoginPageViewModel>();
        services.AddTransient<RegistrationPageViewModel>();
        services.AddTransient<AdminPageViewModel>();
        services.AddTransient<UserPageViewModel>();
        services.AddTransient<ViewModel.ModalWindowsViewModel.RegistrationWindowViewModel>();
        services.AddTransient<ViewModel.ModalWindowsViewModel.DeleteHumanWindowViewModel>();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);

            var mainWindow = new ContainerWindow();
            var navigateService = Services.GetRequiredService<INavigateService>();

            if (navigateService == null)
                throw new InvalidOperationException("NavigateService не зарегистрирован в DI.");

            navigateService.ConfigureNavigation(mainWindow.MainFrame);

            navigateService.Navigated += (pageName) =>
            {
            Page? page = pageName switch
            {
                "LoginPageView" => CreateLoginPage(),
                "RegistrationPageView" => CreateRegistrationPage(),
                "AdminPageView" => CreateAdminPage(),
                "UserPageView" => CreateUserPage(),
                _ => null
            };

                if (page != null)
                    mainWindow.MainFrame.Navigate(page);
            };

            mainWindow.Show();

            navigateService.NavigateTo<LoginPageViewModel>();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ошибка при запуске приложения:\n{ex.Message}\n\nСтек:\n{ex.StackTrace}",
                "Критическая ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Current.Shutdown(-1);
        }
    }

    // Вспомогательные методы!
    private Page CreateLoginPage() =>
        new LoginPageView(Services.GetRequiredService<LoginPageViewModel>());

    private Page CreateRegistrationPage() =>
        new RegistrationPageView(Services.GetRequiredService<RegistrationPageViewModel>());

    private Page CreateAdminPage() =>
        new AdminPageView(Services.GetRequiredService<AdminPageViewModel>());

    private Page CreateUserPage() =>
        new UserPageView(Services.GetRequiredService<UserPageViewModel>());
}