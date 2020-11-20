using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace ConanServerSwitcher
{
	public partial class App
	{
		private TaskbarIcon _taskBar;
		public App()
		{
			if (!OperatingSystem.IsWindows())
			{
				MessageBox.Show("This application will only work on Windows OS, as it uses the Registry.");
			}
		}

		//protected override void OnStartup(StartupEventArgs e)
		//{
		//	base.OnStartup(e);

		//	_taskBar = (TaskbarIcon) FindResource("TaskbarIcon");
		//}

		//protected override void OnExit(ExitEventArgs e)
		//{
		//	_taskBar?.Dispose();

		//	base.OnExit(e);
		//}
	}
}
