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

		public ServerInformationViewModel(IApplicationConfigurationService configurationService)
		{
			_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
		}

		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

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

		public string ModListPath
		{
			get => GetProperty(() => ModListPath);
			set => SetProperty(() => ModListPath, value);
		}

		public ICommand Initialize => new DelegateCommand(ExecuteInitialize);

		private void ExecuteInitialize()
		{
		}

		public ICommand DialogAccept => new DelegateCommand(ExecuteDialogAccept);
		
		public ICommand DialogCancel => new DelegateCommand(ExecuteDialogCancel);

		private void ExecuteDialogAccept()
		{
			var original = _configurationService.CurrentConfiguration.ServerInformation.FirstOrDefault(s => s.Name.Equals(ServerName));
			if(original != null)
			{
				_configurationService.CurrentConfiguration.ServerInformation.Remove(original);
			}

			_configurationService.CurrentConfiguration
			                     .ServerInformation
			                     .Add(new ServerInformation
			                          {
				                          Name    = ServerName,
				                          Address = ServerAddress,
				                          Port    = ServerPort,
				                          ModList = ModListPath
			                          });
			_configurationService.SaveConfiguration();

			CurrentWindowService?.Close();
		}

		private void ExecuteDialogCancel() => CurrentWindowService?.Close();

		protected override void OnParameterChanged(object parameter)
		{
			base.OnParameterChanged(parameter);

			if (parameter is ServerInformation server)
			{
				ServerName = server.Name;
				ServerAddress = server.Address;
				ServerPort = server.Port;
				ModListPath = server.ModList;

			}
		}
	}
}