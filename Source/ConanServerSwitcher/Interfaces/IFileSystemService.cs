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

using System.Collections.Generic;
using System.Text;
// ReSharper disable WordCanBeSurroundedWithMetaTags

namespace ConanServerSwitcher.Interfaces
{
	public interface IFileSystemService
	{
		/// <summary>
		/// Gets the file path to the local application data folder for the application.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		string GetLocalApplicationDataPath(string fileName);

		/// <summary>Checks if a file exists.</summary>
		/// <param name="path">Full <paramref name="path"/> of the file to check.</param>
		/// <returns><c>True</c> if the file exists, otherwise false.</returns>
		bool Exists(string path);

		/// <summary>Reads the content of the defined file with the defined <paramref name="encoding"/>.</summary>
		/// <param name="path"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		string ReadFileContent(string path, Encoding encoding);

		/// <summary>Write the content to the defined file with the defined <paramref name="encoding"/>.</summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		/// <param name="encoding"></param>
		void SaveFileContent(string path, string contents, Encoding encoding);

		/// <summary>Combines all the arguments into a single complete path.</summary>
		/// <param name="args">The arguments.</param>
		/// <returns></returns>
		string GetFullPath(params string[] args);

		/// <summary>
		/// Copies the file.
		/// </summary>
		/// <param name="sourceFile">The source file.</param>
		/// <param name="destinationFile">The destination file.</param>
		void CopyFile(string sourceFile, string destinationFile);

		/// <summary>Gets all files from the <paramref name="path"/> which matches the defined mask.</summary>
		/// <param name="path"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		IEnumerable<string> GetFiles(string path, string mask);

		/// <summary>Returns the full directory from the <paramref name="path"/>.</summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetDirectoryName(string path);
	}
}