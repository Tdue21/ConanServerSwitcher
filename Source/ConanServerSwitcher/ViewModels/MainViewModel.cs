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
using System.Threading.Tasks;
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
			Servers = new ObservableCollection<ServerInformation>
			          {
				          new ServerInformation { Name = "The Green Isle", Address = "89.127.35.254", Port = "7777", ModList = null },
				          new ServerInformation { Name = "Shades & Fangs", Address = "51.89.70.206", Port  = "7777", ModList = null },
			          };
		}

		public ObservableCollection<ServerInformation> Servers { get; set; }

		public ICommand Initialize => new AsyncCommand(ExecuteInitialize);
		
		public ICommand CloseApplication => new AsyncCommand(ExecuteCloseApplication);

		public ICommand AddServer => new AsyncCommand(ExecuteAddServer);

		public ICommand<ServerInformation> EditServer => new AsyncCommand<ServerInformation>(ExecuteEditServer);

		public ICommand<ServerInformation> RemoveServer => new AsyncCommand<ServerInformation>(ExecuteRemoveServer);

		private Task ExecuteInitialize()
		{
			return Task.FromResult(false);
			//throw new NotImplementedException();
		}

		private Task ExecuteCloseApplication()
		{
			throw new NotImplementedException();
		}

		private Task ExecuteAddServer()
		{
			throw new NotImplementedException();
		}

		private Task ExecuteEditServer(ServerInformation arg)
		{
			throw new NotImplementedException();
		}

		private Task ExecuteRemoveServer(ServerInformation arg)
		{
			throw new NotImplementedException();
		}
	}
}