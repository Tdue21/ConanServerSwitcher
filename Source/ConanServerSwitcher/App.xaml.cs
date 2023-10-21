// ****************************************************************************
// * The MIT License(MIT)
// * Copyright © 2020 Thomas Due
// *
// * Permission is hereby granted, free of charge, to any person obtaining a
// * copy of this software and associated documentation files (the “Software”),
// * to deal in the Software without restriction, including without limitation
// * the rights to use, copy, modify, merge, publish, distribute, sublicense,
// * and/or sell copies of the Software, and to permit persons to whom the
// * Software is furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS
// * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// * IN THE SOFTWARE.
// ****************************************************************************

using System;
using System.Globalization;
using System.Windows;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Services;
using ConanServerSwitcher.ViewModels;
using DevExpress.Mvvm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConanServerSwitcher.Views;
using DevExpress.Mvvm.UI;

namespace ConanServerSwitcher;

public partial class App
{
    private readonly IHost _host;
    private IConfigurationRoot _configuration;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration(ConfigureApplication)
                    .ConfigureServices(ConfigureServices)
                    .Build();
    }

    private void ConfigureApplication(HostBuilderContext context, IConfigurationBuilder configBuilder)
    {
        _configuration = configBuilder.Build();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // Services
        services.AddTransient<ISteamLocator, SteamLocator>()
            .AddTransient<IRegistryService, RegistryService>()
            .AddTransient<IFileSystemService, FileSystemService>()
                .AddTransient<IProcessManagementService, ProcessManagementService>()
                .AddTransient<IApplicationConfigurationService, ApplicationConfigurationService>();

        // View Models
        services.AddTransient<MainViewModel>()
                .AddTransient<ServerInformationViewModel>()
                .AddTransient<ApplicationSettingsViewModel>()
                .AddTransient<EnterPromptViewModel>();

        // Views
        services//.AddSingleton<ServerInformationView>()
                //.AddSingleton<ApplicationSettingsView>()
                //.AddSingleton<EnterPromptView>()
                    .AddSingleton<IViewLocator>(ViewLocator.Default)
                    .AddSingleton<IViewModelLocator>(ViewModelLocator.Default)
                .AddSingleton<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var config = _host.Services.GetRequiredService<IApplicationConfigurationService>();
        var data = config.LoadConfiguration();

        Localization.Localization.Culture = CultureInfo.GetCultureInfo(data.SelectedCulture);
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(data.SelectedCulture);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(data.SelectedCulture);

        if (!OperatingSystem.IsWindows())
        {
            MessageBox.Show(
                    Localization.Localization.OnlyOnWindows,
                    Localization.Localization.Warning,
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
            Current.Shutdown();
        }

        var window = _host.Services.GetRequiredService<MainWindow>();
        window.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }
        base.OnExit(e);
    }
}