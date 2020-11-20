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
using System.Linq;
using System.Windows.Input;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Models;
using DevExpress.Mvvm;

namespace ConanServerSwitcher.ViewModels
{
	public class ServerInformationViewModel : ViewModelBase
	{
		private readonly IApplicationConfigurationService _configurationService;
		private ApplicationConfiguration _config;

		public ServerInformationViewModel(IApplicationConfigurationService configurationService)
		{
			_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
		}

		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

		public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();

		public Guid ServerId { get; set; }

		public string ServerName 
		{ 
			get => GetProperty(() => ServerName); 
			set => SetProperty(() => ServerName, value);
		}
		
		public string ServerAddress
		{
			get => GetProperty(() => ServerAddress);
			set => SetProperty(() => ServerAddress, value);
		}

		public string ServerPort
		{
			get => GetProperty(() => ServerPort);
			set => SetProperty(() => ServerPort, value);
		}

		public string Password
		{
			get => GetProperty(() => Password);
			set => SetProperty(() => Password, value);
		}

		public string ModListPath
		{
			get => GetProperty(() => ModListPath);
			set => SetProperty(() => ModListPath, value);
		}

		public bool UseBattleEye
		{
			get => GetProperty(() => UseBattleEye);
			set => SetProperty(() => UseBattleEye, value);
		}

		public ICommand Initialize => new DelegateCommand(ExecuteInitialize);

		public ICommand DialogAccept => new DelegateCommand(ExecuteDialogAccept);
		
		public ICommand DialogCancel => new DelegateCommand(ExecuteDialogCancel);

		public ICommand BrowseForFile => new DelegateCommand(ExecuteBrowseForFile);
															 
		private void ExecuteInitialize() => _config = _configurationService.LoadConfiguration();

		private void ExecuteDialogAccept()
		{
			var original = _config.ServerInformation.FirstOrDefault(s => s.Id.Equals(ServerId));
			if(original == null)
			{
				original = new ServerInformation();
				_config.ServerInformation.Add(original);
			}

			original.Name = ServerName;
			original.Address = ServerAddress;
			original.Port = ServerPort;
			original.Password = Password;
			original.BattlEye = UseBattleEye;
			original.ModList = ModListPath;

			_configurationService.SaveConfiguration(_config);

			CurrentWindowService?.Close();
		}

		private void ExecuteDialogCancel() => CurrentWindowService?.Close();

		private void ExecuteBrowseForFile()
		{
			if (OpenFileDialogService.ShowDialog())
			{
				ModListPath = OpenFileDialogService.GetFullFileName();
			}
		}

		protected override void OnParameterChanged(object parameter)
		{
			base.OnParameterChanged(parameter);

			if (parameter is ServerInformation server)
			{
				ServerId = server.Id;
				ServerName = server.Name;
				ServerAddress = server.Address;
				ServerPort = server.Port;
				ModListPath = server.ModList;
				Password = server.Password;
				UseBattleEye = server.BattlEye;
			}
		}
	}
}