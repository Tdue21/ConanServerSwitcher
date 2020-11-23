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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConanServerSwitcher.Interfaces;
using DevExpress.Mvvm.Native;
using Gameloop.Vdf.Linq;

// ReSharper disable StringLiteralsWordIsNotInDictionary

namespace ConanServerSwitcher.Services
{
	public class SteamLocator : ISteamLocator
	{
		private readonly IRegistryService _registryService;
		private readonly IFileSystemService _fileSystemService;
		private string _installPath;

		public SteamLocator(IRegistryService registryService, IFileSystemService fileSystemService)
		{
			_registryService = registryService ?? throw new ArgumentNullException(nameof(registryService));
			_fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
		}

		private string SteamInstallPath => _installPath ??= (string) _registryService.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath");

		/// <inheritdoc />
		public string GetSteamPath() => _fileSystemService.GetFullPath(SteamInstallPath, "Steam.exe");

		/// <inheritdoc />
		public string GetAppPath(int gameId)
		{
			var folders = GetLibraryFolders().ToList();
			var files = folders.SelectMany(item => _fileSystemService.GetFiles(item, @"appmanifest_*.acf")).ToList();
			var selected = files.FirstOrDefault(file => file.Contains(gameId.ToString()));

			var path = _fileSystemService.GetDirectoryName(selected);
			var installPath = _fileSystemService.GetFullPath(path, "common", "Conan Exiles", "ConanSandbox");


			return installPath;
		}

		public string GetAppPath(int gameId, params string[] subFolders)
		{
			var appPath = GetAppPath(gameId);
			var args = new List<string> { appPath};
			args.AddRange(subFolders);

			return _fileSystemService.GetFullPath(args.ToArray());
		}

		private IEnumerable<string> GetLibraryFolders()
		{
			var original = _fileSystemService.GetFullPath(SteamInstallPath, @"steamapps");

			yield return original;

			var libraryFolders = _fileSystemService.GetFullPath(original, @"libraryfolders.vdf");
			var content        = _fileSystemService.ReadFileContent(libraryFolders, Encoding.UTF8);
			var data           = Gameloop.Vdf.VdfConvert.Deserialize(content);


			foreach (var item in data.Value
				.Children()
				.Cast<VProperty>()
				.Select(token => token.Value.ToString())
				.Where(item => item.IsValidPath())
				.Select(path => _fileSystemService.GetFullPath(path, @"steamapps")))
			{
				yield return item;
			}
		}
	}
}