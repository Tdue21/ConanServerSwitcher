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

using System.Text;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Services;
using ConanServerSwitcher.UnitTests.Properties;
using ConanServerSwitcher.ViewModels;
using FluentAssertions;
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
            vm.SteamExe   = "C:\\Something\\Steam.exe";
            vm.GameFolder = "C:\\GameFolder";

            vm.DialogAccept.Execute(null);

            //cs.CurrentConfiguration.GameFolder.Should().Be("C:\\GameFolder");
            //cs.CurrentConfiguration.SteamExecutable.Should().Be("C:\\Something\\Steam.exe");
        }

        [Fact]
        public void InitializeAndAccept_With_Previous_SaveFile_Test()
        {
            var fs = new Mock<IFileSystemService>();
            fs.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            fs.Setup(f => f.ReadFileContent(It.IsAny<string>(), It.IsAny<Encoding>())).Returns(Resources.ConanServerSwitcherSettings);

            var cs = new ApplicationConfigurationService(fs.Object);
            var vm = new ApplicationSettingsViewModel(cs);

            vm.Initialize.Execute(null);
            vm.SteamExe   = "C:\\Something\\Steam.exe";
            vm.GameFolder = "C:\\GameFolder";

            vm.DialogAccept.Execute(null);

            //cs.CurrentConfiguration.GameFolder.Should().Be("C:\\GameFolder");
            //cs.CurrentConfiguration.SteamExecutable.Should().Be("C:\\Something\\Steam.exe");
        }
    }
}
