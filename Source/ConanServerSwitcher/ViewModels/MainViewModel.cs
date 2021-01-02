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
using GongSolutions.Wpf.DragDrop;

namespace ConanServerSwitcher.ViewModels
{
	public class MainViewModel : ViewModelBase, IDropTarget
	{
		private readonly IApplicationConfigurationService _configurationService;
		private readonly IProcessManagementService _processManagementService;
		private readonly IFileSystemService _fileSystem;
		private readonly ISteamLocator _steamLocator;
		private readonly IViewModelLocator _vmLocator;
		private ApplicationConfiguration _config;

		public MainViewModel(IApplicationConfigurationService configurationService, IProcessManagementService processManagementService, IFileSystemService fileSystem, ISteamLocator steamLocator, IViewModelLocator vmLocator)
		{
			_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
			_processManagementService = processManagementService ?? throw new ArgumentNullException(nameof(processManagementService));
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			_steamLocator = steamLocator ?? throw new ArgumentNullException(nameof(steamLocator));
			_vmLocator = vmLocator ?? throw new ArgumentNullException(nameof(vmLocator));

			Servers = new ObservableCollection<ServerInformation>();
		}

		public ObservableCollection<ServerInformation> Servers { get; set; }

		public IWindowService EditServerWindow => GetService<IWindowService>("EditServerWindow");

		public IWindowService EnterPromptWindow => GetService<IWindowService>("EnterPromptWindow");

		public IWindowService ApplicationSettingsWindow => GetService<IWindowService>("ApplicationSettingsWindow");

		public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
		
		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();
		
		public ICommand Initialize => new DelegateCommand(ExecuteInitialize);
		
		public ICommand SettingsDialog => new DelegateCommand(ExecuteSettingsDialog);
		
		public ICommand CopyModList => new DelegateCommand(ExecuteCopyModList);

		public ICommand CloseApplication => new DelegateCommand(ExecuteCloseApplication);

		public ICommand AddServer => new DelegateCommand(() => ExecuteEditServer(new ServerInformation()));

		public ICommand<ServerInformation> RunGame => new DelegateCommand<ServerInformation>(ExecuteRunGame, s => s != null);

		public ICommand<ServerInformation> EditServer => new DelegateCommand<ServerInformation>(ExecuteEditServer, s => s != null);

		public ICommand<ServerInformation> RemoveServer => new DelegateCommand<ServerInformation>(ExecuteRemoveServer, s => s != null);

		private void ExecuteCloseApplication() => CurrentWindowService?.Close();

		public ServerInformation SelectedServer
		{
			get => GetProperty(() => SelectedServer);
			set => SetProperty(() => SelectedServer, value);
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
			if (MessageBoxService.Accept(Localization.Localization.StartGame, Localization.Localization.AreYouSureYouWishToStart))
			{
				_processManagementService.StartProcess(_config.SteamExecutable, _config.GameFolder, arg);
			}
		}

		private void ExecuteCopyModList()
		{
			var vm = _vmLocator.ResolveViewModel<EnterPromptViewModel>();
			if (vm != null)
			{
				vm.Prompt = Localization.Localization.EnterModListNamePrompt;
				vm.Value = "";

				EnterPromptWindow.Show("", vm);
				var result = vm.Value;

				if (!string.IsNullOrWhiteSpace(result))
				{
					// ReSharper disable once StringLiteralTypo
					var sourcePath = _steamLocator.GetAppPath(440900, @"servermodlist.txt");
					if (!string.IsNullOrWhiteSpace(sourcePath) && _fileSystem.FileExists(sourcePath))
					{

						var modPath = _steamLocator.GetAppPath(440900, "mods");
						if (!_fileSystem.PathExists(modPath))
						{
							_fileSystem.CreatePath(modPath);
						}

						if (!result.EndsWith(".txt"))
						{
							result += ".txt";
						}

						var destinationPath = _fileSystem.GetFullPath(modPath, result);
						var doAction = !(_fileSystem.FileExists(destinationPath) && !MessageBoxService.Accept(Localization.Localization.FileExists,
								                 string.Format(Localization.Localization.OverwriteDestinationFile, destinationPath)));

						if (doAction)
						{
							_fileSystem.CopyFile(sourcePath, destinationPath);

							MessageBoxService.Information(
									Localization.Localization.ActionCompleted,
									string.Format(Localization.Localization.ServerModListCopied, destinationPath));
						}
						else
						{
							MessageBoxService.Warning(
									Localization.Localization.Warning, 
									Localization.Localization.ActionNotCompleted);
						}
					}
				}
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
			if (MessageBoxService.Accept(Localization.Localization.DeleteServerEntryCaption, Localization.Localization.DeleteServerEntryMessage))
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

		private readonly DefaultDropHandler _dropHandler = new DefaultDropHandler();
		
		void IDropTarget.DragOver(IDropInfo dropInfo) => _dropHandler.DragOver(dropInfo);

		void IDropTarget.Drop(IDropInfo dropInfo)
		{
			_dropHandler.Drop(dropInfo);

			_config.ServerInformation.Clear();
			_config.ServerInformation.AddRange(Servers);
			_configurationService.SaveConfiguration(_config);
		}
	}
}