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
		private readonly IProcessManagementService _processManagementService;
		private readonly ISteamLocator _steamLocator;
		private ApplicationConfiguration _config;

		public MainViewModel(IApplicationConfigurationService configurationService, IProcessManagementService processManagementService, ISteamLocator steamLocator)
		{
			_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
			_processManagementService = processManagementService ?? throw new ArgumentNullException(nameof(processManagementService));
			_steamLocator = steamLocator ?? throw new ArgumentNullException(nameof(steamLocator));

			Servers = new ObservableCollection<ServerInformation>();
		}

		public ObservableCollection<ServerInformation> Servers { get; set; }

		public IWindowService EditServerWindow => GetService<IWindowService>("EditServerWindow");
		
		public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

		public IWindowService ApplicationSettingsWindow => GetService<IWindowService>("ApplicationSettingsWindow");
		
		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();
		
		public ICommand Initialize => new DelegateCommand(ExecuteInitialize);
		
		public ICommand SettingsDialog => new DelegateCommand(ExecuteSettingsDialog);

		public ICommand CloseApplication => new DelegateCommand(ExecuteCloseApplication);

		public ICommand AddServer => new DelegateCommand(() => ExecuteEditServer(new ServerInformation()));

		public ICommand<ServerInformation> RunGame => new DelegateCommand<ServerInformation>(ExecuteRunGame);

		public ICommand<ServerInformation> EditServer => new DelegateCommand<ServerInformation>(ExecuteEditServer);

		public ICommand<ServerInformation> RemoveServer => new DelegateCommand<ServerInformation>(ExecuteRemoveServer);

		private bool AcceptMessageBox(string caption, string message) => MessageBoxService.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

		private void ExecuteCloseApplication()
		{
			if (CurrentWindowService != null)
			{
				CurrentWindowService.Close();
			}
			else
			{
				// TODO: Currently necessary for the notify icon.
				Application.Current.Shutdown(0);
			}
		}

		private void ExecuteInitialize()
		{
			var doSave = false;
			_config = _configurationService.LoadConfiguration();
			if (string.IsNullOrWhiteSpace(_config.SteamExecutable))
			{
				_config.SteamExecutable = _steamLocator.GetSteamPath();
				doSave = true;
			}

			if (string.IsNullOrWhiteSpace(_config.GameFolder))
			{
				_config.GameFolder = _steamLocator.GetAppPath(440900);
				doSave = true;
			}

			if (doSave)
			{
				_configurationService.SaveConfiguration(_config);
			}

			Servers.Clear();
			foreach (var information in _config.ServerInformation)
			{
				Servers.Add(information);
			}
		}

		private void ExecuteRunGame(ServerInformation arg)
		{
			if (AcceptMessageBox(Localization.Localization.StartGame, Localization.Localization.AreYouSureYouWishToStart))
			{
				_processManagementService.StartProcess(_config.SteamExecutable, _config.GameFolder, arg);
			}
		}

		private void ExecuteSettingsDialog()
		{
			ApplicationSettingsWindow?.Show(null);
			ExecuteInitialize();
		}

		private void ExecuteEditServer(ServerInformation arg)
		{
			EditServerWindow?.Show(null, arg, this);
			ExecuteInitialize();
		}

		private void ExecuteRemoveServer(ServerInformation arg)
		{
			if (AcceptMessageBox(Localization.Localization.DeleteServerEntryCaption, Localization.Localization.DeleteServerEntryMessage))
			{
				var result = _config.ServerInformation.FirstOrDefault(i => i.Equals(arg));
				if (result != null)
				{
					Servers.Remove(result);
					_config.ServerInformation.Remove(result);
					_configurationService.SaveConfiguration(_config);
				}
			}
		}
	}
}