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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Models;
using DevExpress.Mvvm;

namespace ConanServerSwitcher.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IApplicationConfigurationService _configurationService;

		public MainViewModel(IApplicationConfigurationService configurationService)
		{
			_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
			Servers = new ObservableCollection<ServerInformation>();
		}

		public ObservableCollection<ServerInformation> Servers { get; set; }

		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

		public IWindowService EditServerWindow => GetService<IWindowService>("EditServerWindow");
		
		public IWindowService ApplicationSettingsWindow => GetService<IWindowService>("ApplicationSettingsWindow");
		
		public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

		public ICommand Initialize => new DelegateCommand(ExecuteInitialize);
		
		public ICommand SettingsDialog => new DelegateCommand(ExecuteSettingsDialog);

		public ICommand CloseApplication => new DelegateCommand(ExecuteCloseApplication);

		public ICommand AddServer => new DelegateCommand(ExecuteAddServer);

		public ICommand<ServerInformation> EditServer => new DelegateCommand<ServerInformation>(ExecuteEditServer);

		public ICommand<ServerInformation> RemoveServer => new DelegateCommand<ServerInformation>(ExecuteRemoveServer);

		private void ExecuteInitialize()
		{
			_configurationService.LoadConfiguration();
			Servers.Clear();
			foreach (var information in _configurationService.CurrentConfiguration.ServerInformation)
			{
				Servers.Add(information);
			}
		}

		private void ExecuteCloseApplication() => CurrentWindowService?.Close();

		private void ExecuteSettingsDialog()
		{
			ApplicationSettingsWindow?.Show(null);
		}

		private void ExecuteAddServer() => ExecuteEditServer(new ServerInformation());

		private void ExecuteEditServer(ServerInformation arg)
		{
			EditServerWindow?.Show(null, arg, this);
			ExecuteInitialize();
		}

		private void ExecuteRemoveServer(ServerInformation arg)
		{
			if (AcceptMessageBox("Delete server entry", "Are you sure you wish to the delete this server entry?"))
			{
				var result = _configurationService.CurrentConfiguration.ServerInformation.FirstOrDefault(i => i.Equals(arg));
				if (result != null)
				{
					_configurationService.CurrentConfiguration.ServerInformation.Remove(result);
					_configurationService.SaveConfiguration();
				}
			}
		}

		private bool AcceptMessageBox(string caption, string message) => MessageBoxService.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
	}
}