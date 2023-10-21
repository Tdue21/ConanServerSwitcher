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
using System.Globalization;
using System.Linq;
using System.Text;
using ConanServerSwitcher.Interfaces;
using ConanServerSwitcher.Models;
using Newtonsoft.Json;

namespace ConanServerSwitcher.Services;

public class ApplicationConfigurationService : IApplicationConfigurationService
{
    private const string ConfigFile = "ConanServerSwitcherSettings.json";

    private readonly IFileSystemService _fileSystemService;
    private readonly string _configurationFile;

    public ApplicationConfigurationService(IFileSystemService fileSystemService)
    {
        _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        _configurationFile = _fileSystemService.GetLocalApplicationDataPath(ConfigFile);
    }

    public ApplicationConfiguration LoadConfiguration()
    {
        if (!_fileSystemService.Exists(_configurationFile))
        {
            return new ApplicationConfiguration();
        }

        var data = _fileSystemService.ReadFileContent(_configurationFile, Encoding.UTF8);
        var result = JsonConvert.DeserializeObject<ApplicationConfiguration>(data);

        foreach (var server in result.ServerInformation.Where(server => server.Id.Equals(Guid.Empty)))
        {
            server.Id = Guid.NewGuid();
        }

        return result.Clone();
    }

    public void SaveConfiguration(ApplicationConfiguration data)
    {
        var result = JsonConvert.SerializeObject(data, Formatting.Indented);
        _fileSystemService.SaveFileContent(_configurationFile, result, Encoding.UTF8);
    }

    public List<CultureInfo> GetAvailableCultures()
    {
        return new List<CultureInfo>
            {
                    CultureInfo.GetCultureInfo("en"),
                    CultureInfo.GetCultureInfo("da")
            };
    }
}