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

using System.IO;
using System.Text;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Models;
using ConanServerSwitcher.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConanServerSwitcher.UnitTests
{
	public class ApplicationConfigurationServiceTests
	{
		[Fact]
		public void LoadSettings_No_Existing_Settings_Test()
		{
			var expected = new ApplicationConfiguration();

			var fs = new Mock<IFileSystemService>();
			fs.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);

			var cs = new ApplicationConfigurationService(fs.Object);

			var result = cs.LoadConfiguration();

			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void LoadSettings_Existing_Settings_Test()
		{
			var expected = new ApplicationConfiguration
			{
				SteamExecutable = "C:\\Program Files (x86)\\Steam\\Steam.exe",
				GameFolder = "C:\\Program Files (x86)\\Steam\\SteamLibrary\\ConanExiles",
				ServerInformation =
				{
					new ServerInformation { Name = "Shades & Fangs",        Address = "51.89.70.206",  Port = "7777", ModList = null },
					new ServerInformation { Name = "The Green Isle",        Address = "89.127.35.254", Port = "7777", ModList = null },
					new ServerInformation { Name = "Rise of the Overlords", Address = "12.13.14.15",   Port = "12345", ModList = null },
				}
			};

			var settingsData = File.ReadAllText("Assets/ConanServerSwitcherSettings.json");
			var fs = new Mock<IFileSystemService>();
			fs.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
			fs.Setup(f => f.ReadFileContent(It.IsAny<string>(), It.IsAny<Encoding>())).Returns(settingsData);

			var cs = new ApplicationConfigurationService(fs.Object);

			var result = cs.LoadConfiguration();

			result.Should().BeEquivalentTo(expected);
		}
	}
}