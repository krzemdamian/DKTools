﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RevitDKTools.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RevitDKTools.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to DynamicProxy.
        /// </summary>
        internal static string DYNAMIC_ASSEMBLY_NAME {
            get {
                return ResourceManager.GetString("DYNAMIC_ASSEMBLY_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \PythonScripts\.
        /// </summary>
        internal static string PROXY_DLL_LOCATION {
            get {
                return ResourceManager.GetString("PROXY_DLL_LOCATION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \PythonScripts\.
        /// </summary>
        internal static string SCRIPTS_FOLDER_LOCATION {
            get {
                return ResourceManager.GetString("SCRIPTS_FOLDER_LOCATION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \PythonScripts\ScriptsSettings.xml.
        /// </summary>
        internal static string SCRIPTS_SETTINGS_XML_LOCATION {
            get {
                return ResourceManager.GetString("SCRIPTS_SETTINGS_XML_LOCATION", resourceCulture);
            }
        }
    }
}
