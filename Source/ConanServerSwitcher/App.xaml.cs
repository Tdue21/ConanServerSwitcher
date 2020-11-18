using System;
using System.Windows;

namespace ConanServerSwitcher
{
	public partial class App 
	{
		public App()
		{
			if (!OperatingSystem.IsWindows())
			{
				MessageBox.Show("This application will only work on Windows OS, as it uses the Registry.");
			}
		}
	}
}
