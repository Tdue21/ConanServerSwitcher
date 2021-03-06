﻿// ****************************************************************************
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
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows.Input;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Models;
using DevExpress.Mvvm;

namespace ConanServerSwitcher.ViewModels
{
	public class ApplicationSettingsViewModel : ViewModelBase
	{
		private readonly IApplicationConfigurationService _configurationService;
		private ApplicationConfiguration _config;

		public ApplicationSettingsViewModel(IApplicationConfigurationService configurationService)
		{
			_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
		}
		
		public ICommand Initialize => new DelegateCommand(ExecuteInitialize);

		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

		public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
		
		public IFolderBrowserDialogService FolderBrowserDialogService => GetService<IFolderBrowserDialogService>();

		public ICommand BrowseForFile => new DelegateCommand(ExecuteBrowseForFile);

		public ICommand BrowseForFolder => new DelegateCommand(ExecuteBrowseForFolder);

		public ICommand DialogAccept => new DelegateCommand(ExecuteDialogAccept);

		public ICommand DialogCancel => new DelegateCommand(ExecuteDialogCancel);

		public ObservableCollection<CultureInfo> AvailableCultures
		{
			get => GetProperty(() => AvailableCultures);
			set => SetProperty(() => AvailableCultures, value);
		}

		public CultureInfo SelectedCulture
		{
			get => GetProperty(() => SelectedCulture);
			set => SetProperty(() => SelectedCulture, value);
		}
		public string SteamExe 
		{ 
			get => GetProperty(() => SteamExe); 
			set => SetProperty(() => SteamExe, value);
		}

		public string GameFolder
		{
			get => GetProperty(() => GameFolder);
			set => SetProperty(() => GameFolder, value);
		}

		private void ExecuteInitialize()
		{
			_config = _configurationService.LoadConfiguration();

			AvailableCultures = new ObservableCollection<CultureInfo>(_configurationService.GetAvailableCultures());

			SelectedCulture =
					AvailableCultures.FirstOrDefault(c => c.TwoLetterISOLanguageName == _config.SelectedCulture);
			SteamExe = _config.SteamExecutable;
			GameFolder = _config.GameFolder;
		}

		private void ExecuteBrowseForFile()
		{
			if (OpenFileDialogService.ShowDialog())
			{
				SteamExe = OpenFileDialogService.GetFullFileName();
			}
		}

		private void ExecuteBrowseForFolder()
		{
			if (FolderBrowserDialogService.ShowDialog())
			{
				GameFolder = FolderBrowserDialogService.ResultPath;
			}
		}

		private void ExecuteDialogAccept()
		{
			_config.SteamExecutable = SteamExe;
			_config.GameFolder = GameFolder;
			_config.SelectedCulture = SelectedCulture.TwoLetterISOLanguageName;
			_configurationService.SaveConfiguration(_config);

			CurrentWindowService?.Close();
		}

		private void ExecuteDialogCancel() => CurrentWindowService?.Close();
	}
}