﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConanServerSwitcher.UnitTests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ConanServerSwitcher.UnitTests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;SteamExecutable&quot;: &quot;C:\\Program Files (x86)\\Steam\\Steam.exe&quot;,
        ///  &quot;ConanInstallPath&quot;: &quot;C:\\Program Files (x86)\\Steam\\SteamLibrary\\ConanExiles&quot;,
        ///  &quot;ServerInformation&quot;: [
        ///    { &quot;Name&quot;: &quot;Shades &amp; Fangs&quot;, &quot;Address&quot;: &quot;51.89.70.206&quot;, &quot;Port&quot;: &quot;7777&quot;, &quot;ModList&quot;: null },
        ///    { &quot;Name&quot;: &quot;The Green Isle&quot;, &quot;Address&quot;: &quot;89.127.35.254&quot;, &quot;Port&quot;: &quot;7777&quot;, &quot;ModList&quot;: null },
        ///    { &quot;Name&quot;: &quot;Rise of the Overlords&quot;, &quot;Address&quot;: &quot;12.13.14.15&quot;, &quot;Port&quot;: &quot;12345&quot;, &quot;ModList&quot;: null }
        ///  ]
        ///}.
        /// </summary>
        internal static string ConanServerSwitcherSettings {
            get {
                return ResourceManager.GetString("ConanServerSwitcherSettings", resourceCulture);
            }
        }
    }
}