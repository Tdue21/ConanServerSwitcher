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
using System.IO;
using System.Text;
using ConanServerSwitcher.Interfaces;

namespace ConanServerSwitcher.Services;

public class FileSystemService : IFileSystemService
{
    /// <inheritdoc />
    public string GetLocalApplicationDataPath(string fileName)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (Directory.Exists(path))
        {
            var filePath = Path.Combine(path, "ConanServerSwitcher");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return Path.Combine(filePath, fileName);
        }
        return fileName;
    }

    /// <inheritdoc />
    public bool Exists(string path) => File.Exists(path);

    /// <inheritdoc />
    public string ReadFileContent(string path, Encoding encoding) => File.ReadAllText(path, encoding);

    /// <inheritdoc />
    public void SaveFileContent(string path, string contents, Encoding encoding) => File.WriteAllText(path, contents, encoding);

    /// <inheritdoc />
    public string GetFullPath(params string[] args) => Path.Combine(args);

    /// <inheritdoc />
    public void CopyFile(string sourceFile, string destinationFile) => File.Copy(sourceFile, destinationFile, true);

    /// <inheritdoc />
    public IEnumerable<string> GetFiles(string path, string mask) => Directory.GetFiles(path, mask, SearchOption.TopDirectoryOnly);

    /// <inheritdoc />
    public string GetDirectoryName(string path) => Path.GetDirectoryName(path);

    /// <inheritdoc />
    public bool PathExists(string path) => Directory.Exists(path);

    /// <inheritdoc />
    public void CreatePath(string path) => Directory.CreateDirectory(path);

    /// <inheritdoc />
    public bool FileExists(string path) => File.Exists(path);
}