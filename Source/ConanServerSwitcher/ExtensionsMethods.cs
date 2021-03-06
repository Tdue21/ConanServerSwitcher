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

using System;
using System.IO;
using System.Windows;
using DevExpress.Mvvm;

namespace ConanServerSwitcher
{
	public static class ExtensionsMethods
	{
		public static bool IsValidPath(this string path)
		{
			FileInfo fi = null;
			try
			{
				var uri = new Uri(path, UriKind.Absolute);
				if (uri.IsFile && uri.Scheme == "file")
				{
					fi = new FileInfo(path);
				}

			}
			catch (ArgumentException) { }
			catch (UriFormatException) { }
			catch (PathTooLongException) { }
			catch (NotSupportedException) { }

			return !ReferenceEquals(fi, null);
		}

		public static T ResolveViewModel<T>(this IViewModelLocator vml) => (T)vml?.ResolveViewModel(typeof(T).Name);

		public static void Warning(this IMessageBoxService messageBox, string caption, string message) => 
				messageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

		public static void Information(this IMessageBoxService messageBox, string caption, string message) =>
				messageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);

		public static bool Accept(this IMessageBoxService messageBox, string caption, string message) =>
				messageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

	}
}