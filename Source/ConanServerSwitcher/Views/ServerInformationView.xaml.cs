using System.Windows;
using ConanServerSwitcher.ViewModels;

namespace ConanServerSwitcher.Views;

public partial class ServerInformationView
{
    public ServerInformationView() 
    {
        InitializeComponent();
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
	        if (DataContext is ServerInformationViewModel vm)
	        {
		        vm.Password = ServerPasswordBox.Password;
	        }
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
	        if (DataContext is ServerInformationViewModel vm && vm.Password != null)
	        {
		        ServerPasswordBox.Password = vm.Password;
	        }
    }
}
