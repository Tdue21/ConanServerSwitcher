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
using System.Collections.Generic;
using System.Linq;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace ConanServerSwitcher.Models
{
	public class ApplicationConfiguration : ICloneable
	{
		public ApplicationConfiguration()
		{
			ServerInformation = new List<ServerInformation>();
			SelectedCulture = "en";
		}

		public string SteamExecutable { get; set; }

		public string GameFolder { get; set; }

		public string SelectedCulture { get; set; }

		public List<ServerInformation> ServerInformation { get; }

		object ICloneable.Clone() => MemberwiseClone();

		public ApplicationConfiguration Clone() => (ApplicationConfiguration)MemberwiseClone();

		private bool Equals(ApplicationConfiguration other)
		{
			var result = SteamExecutable == other.SteamExecutable && GameFolder == other.GameFolder;
			var first = ServerInformation.Except(other.ServerInformation);
			var second = other.ServerInformation.Except(ServerInformation);

			return result && !first.Any() && !second.Any();
		}

		public override bool Equals(object obj) => !ReferenceEquals(null, obj) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((ApplicationConfiguration) obj));

		public override int GetHashCode() => HashCode.Combine(SteamExecutable, GameFolder, ServerInformation);
	}
}