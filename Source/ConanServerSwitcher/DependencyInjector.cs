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

using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Services;
using ConanServerSwitcher.ViewModels;
using Unity;
using Unity.Lifetime;

namespace ConanServerSwitcher
{
	public class DependencyInjector
	{
		private readonly IUnityContainer _container;

		public DependencyInjector()
		{
			_container = new UnityContainer().RegisterType<IFileSystemService, FileSystemService>(new ContainerControlledLifetimeManager())
			                                 .RegisterType<IRegistryService, RegistryService>(new ContainerControlledLifetimeManager())
			                                 .RegisterType<ISteamLocator, SteamLocator>(new ContainerControlledLifetimeManager())
			                                 .RegisterType<IProcessManagementService, ProcessManagementService>(new ContainerControlledLifetimeManager())
			                                 .RegisterType<IApplicationConfigurationService, ApplicationConfigurationService>(new ContainerControlledLifetimeManager())
											 
			                                 .RegisterType<MainViewModel>()
			                                 .RegisterType<ServerInformationViewModel>()
			                                 .RegisterType<ApplicationSettingsViewModel>()
				;
		}

		public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();
		
		public ServerInformationViewModel ServerInformationViewModel => _container.Resolve<ServerInformationViewModel>();
		
		public ApplicationSettingsViewModel ApplicationSettingsViewModel => _container.Resolve<ApplicationSettingsViewModel>();


	}
}