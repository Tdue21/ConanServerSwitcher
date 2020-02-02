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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool Exists(string path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		string ReadFileContent(string path, Encoding encoding);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		/// <param name="encoding"></param>
		void SaveFileContent(string path, string contents, Encoding encoding);
	}
}