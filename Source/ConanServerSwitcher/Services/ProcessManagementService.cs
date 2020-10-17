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
using System.Diagnostics;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Models;

namespace ConanServerSwitcher.Services
{
	public class ProcessManagementService : IProcessManagementService
	{
		private readonly IFileSystemService _fileSystemService;

		public ProcessManagementService(IFileSystemService fileSystemService)
		{
			_fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
		}

		public bool StartProcess(string executable, string gameFolder, ServerInformation args)
		{
			if (string.IsNullOrWhiteSpace(executable))
			{
				throw new ArgumentNullException(nameof(executable));
			}

			if (args == null)
			{
				throw new ArgumentNullException(nameof(args));
			}

			var destination = _fileSystemService.GetFullPath(gameFolder, "mods", "modlist.txt");
			_fileSystemService.CopyFile(args.ModList, destination);


			var info = new ProcessStartInfo
					   {
						   FileName = executable, 
						   Arguments = args.ToArgs(),
						   CreateNoWindow = true,
						   UseShellExecute = false
					   };

			using var proc = new Process
			{
				StartInfo = info
			};

			return proc.Start();
		}
	}
}
