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

// ReSharper disable StringLiteralsWordIsNotInDictionary
// ReSharper disable NonReadonlyMemberInGetHashCode

using System;

namespace ConanServerSwitcher.Models
{
	/// <summary>
	/// Class representing a single item in the list of servers registered in the application.
	/// </summary>
	public class ServerInformation : ICloneable
	{
		/// <summary>Gets or sets the name of the server.</summary>
		public string Name { get; set; }
		
		/// <summary>Gets or sets the address of the server.</summary>
		public string Address { get; set; }
		
		/// <summary>Gets or sets the game port for the server.</summary>
		public string Port { get; set; }
		
		/// <summary>Gets or sets the path to the server's modlist.</summary>
		public string ModList { get; set; }

		object ICloneable.Clone() => MemberwiseClone();
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ServerInformation Clone() => (ServerInformation) MemberwiseClone();

		private bool Equals(ServerInformation other) => Name == other.Name;

		public override bool Equals(object obj) => !ReferenceEquals(null, obj) && 
		                                           (ReferenceEquals(this, obj) || 
		                                            obj.GetType() == GetType() && 
		                                            Equals((ServerInformation) obj));

		public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;

		public string ToArgs() => $"-appLaunch 440900 +connect {Address}:{Port}";
	}
}