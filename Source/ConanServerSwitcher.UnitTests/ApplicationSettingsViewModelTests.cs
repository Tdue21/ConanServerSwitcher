using System;
using System.IO;
using System.Text;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Services;
using ConanServerSwitcher.ViewModels;
using Moq;
using Xunit;

namespace ConanServerSwitcher.UnitTests
{
	public class ApplicationSettingsViewModelTests
	{
		[Fact]
		public void InitializeAndAccept_No_Previous_SaveFile_Test()
		{
			var fs = new Mock<IFileSystemService>();
			fs.Setup(f => f.Exists(It.IsAny<string>())).Returns(false);

			var cs = new ApplicationConfigurationService(fs.Object);
			var vm = new ApplicationSettingsViewModel(cs);

			vm.Initialize.Execute(null);
			vm.SteamExe = "C:\\Something\\Steam.exe";
			vm.GameFolder = "C:\\GameFolder";

			vm.DialogAccept.Execute(null);

			//cs.CurrentConfiguration.GameFolder.Should().Be("C:\\GameFolder");
			//cs.CurrentConfiguration.SteamExecutable.Should().Be("C:\\Something\\Steam.exe");
		}

		[Fact]
		public void InitializeAndAccept_With_Previous_SaveFile_Test()
		{
			var settingsData = File.ReadAllText("Assets/ConanServerSwitcherSettings.json");

			var fs = new Mock<IFileSystemService>();
			fs.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
			fs.Setup(f => f.ReadFileContent(It.IsAny<string>(), It.IsAny<Encoding>())).Returns(settingsData);

			var cs = new ApplicationConfigurationService(fs.Object);
			var vm = new ApplicationSettingsViewModel(cs);

			vm.Initialize.Execute(null);
			vm.SteamExe = "C:\\Something\\Steam.exe";
			vm.GameFolder = "C:\\GameFolder";

			vm.DialogAccept.Execute(null);

			//cs.CurrentConfiguration.GameFolder.Should().Be("C:\\GameFolder");
			//cs.CurrentConfiguration.SteamExecutable.Should().Be("C:\\Something\\Steam.exe");
		}
	}
}
